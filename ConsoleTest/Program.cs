using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using ApplicationLayer.InstructionGeneration.Requests;
using Domain.Entities;
using Domain.ReferenceData;
using Infrastructure.InstructionGeneration.DeviceParams;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

internal static class Program
{
    /// <summary>
    /// Точка входа консольного теста генератора указаний.
    /// Демонстрация «правильного» пайплайна под сценарий
    /// «Вывод ВЛ с замыканием поля».
    /// </summary>
    private static async Task Main()
    {
        Console.WriteLine("=== ConsoleTest: LineWithdrawalWithFieldClosing ===");

        var connectionString =
            "Host=localhost;Port=5432;Database=vkr_it;Username=vkr_it_app;Password=VKRitAPP12345671";

        var dbOptions = new DbContextOptionsBuilder<VkrItDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsAssembly("Infrastructure"))
            .UseSnakeCaseNamingConvention()
            .Options;

        await using var db = new VkrItDbContext(dbOptions);

        await db.Database.MigrateAsync();

        // ВАЖНО:
        // По умолчанию ConsoleTest НЕ должен сидировать и НЕ должен изменять БД,
        // иначе вы получаете «почему snapshot не подтягивает изменения» — изменения просто затираются сидом.
        //
        // Чтобы явно сидировать тестовые данные, установите переменную окружения:
        // VKR_IT_CONSOLETEST_SEED=1
        var shouldSeed = string.Equals(
            Environment.GetEnvironmentVariable("VKR_IT_CONSOLETEST_SEED"),
            "1",
            StringComparison.Ordinal);

        long deviceId;
        if (shouldSeed)
        {
            deviceId = await EnsureMinimalDeviceWithFunctionsAsync(db);

            // После сидирования/нормализации очищаем ChangeTracker,
            // чтобы дальнейшее чтение не вернуло «старую» tracked-сущность.
            db.ChangeTracker.Clear();
        }
        else
        {
            // Работает с тем, что реально лежит в БД.
            // При необходимости — поменяйте на нужный deviceId.
            deviceId = 1;
        }

        // 1) Считываем snapshot (правильная интеграция: reader на устройство)
        var reader = new EfCoreDeviceParamsReader(db);
        var snapshot = await reader.ReadAsync(deviceId, CancellationToken.None);

        Console.WriteLine();
        Console.WriteLine($"Snapshot: DeviceId={snapshot.DeviceId}, LineEndId={snapshot.LineEndId}, Name={snapshot.DeviceName}");
        Console.WriteLine($"Snapshot: VtSwitchTrue={snapshot.VtSwitchTrue}");
        Console.WriteLine($"Snapshot: NeedDisconnectLineCTFromDzo={snapshot.NeedDisconnectLineCTFromDzo}");
        Console.WriteLine($"Snapshot: NeedDisableUpaskReceivers={snapshot.NeedDisableUpaskReceivers}");
        Console.WriteLine($"Snapshot: CtRemainsEnergizedOnThisSide={snapshot.CtRemainsEnergizedOnThisSide}");
        Console.WriteLine($"Snapshot: IsFieldClosingAllowed={snapshot.IsFieldClosingAllowed}");
        Console.WriteLine($"Snapshot: VT Main='{snapshot.Vts.Main.Name}', Place='{snapshot.Vts.Main.Place}', PlaceCode='{snapshot.Vts.Main.PlaceCode}'");
        Console.WriteLine($"Snapshot: VT Reserve='{snapshot.Vts.Reserve.Name}', Place='{snapshot.Vts.Reserve.Place}', PlaceCode='{snapshot.Vts.Reserve.PlaceCode}'");
        Console.WriteLine($"Snapshot: CT Place='{snapshot.CtPlace.Place}', PlaceCode='{snapshot.CtPlace.PlaceCode}'");

        // 2) Формируем request (то, что задаёт диспетчер через UI)
        var request = new LineOperationRequest
        {
            LineCode = "VL-500-01",
            Side = SideOfLine.A,
            ActionCode = ActionCode.LineWithdrawalWithFieldClosing,
            FunctionStates = new FunctionStatesRequest
            {
                DfzEnabled = true,
                DzlEnabled = true,
                DzEnabled = true,
                OapvEnabled = true,
                TapvEnabled = true
            }
        };

        // 3) Собираем criteria (БЕЗ ручных with-оверрайдов)
        var builder = new LineOperationCriteriaBuilder();
        var lineEndId = checked((int)snapshot.LineEndId);
        var criteria = builder.Build(request, lineEndId, snapshot);

        Console.WriteLine($"REQ: DFZ={request.FunctionStates.DfzEnabled}, DZL={request.FunctionStates.DzlEnabled}, DZ={request.FunctionStates.DzEnabled}");
        Console.WriteLine($"CRT: DFZ={criteria.DFZEnabled}, DZL={criteria.DZLEnabled}, DZ={criteria.DZEnabled}");

        Console.WriteLine();
        Console.WriteLine("Criteria (summary):");
        Console.WriteLine($" ActionCode={criteria.ActionCode}, Side={criteria.Side}, LineCode={criteria.LineCode}");
        Console.WriteLine($" IsFieldClosingAllowed={criteria.IsFieldClosingAllowed}");

        Console.WriteLine($" CtPlace={criteria.CtPlace}, CtPlaceCode={criteria.CtPlaceCode}");
        Console.WriteLine($" DeviceConnectedToLineCT={criteria.DeviceConnectedToLineCT}");

        Console.WriteLine($" VtSwitchTrue={criteria.VtSwitchTrue}");
        Console.WriteLine($" MainVtPlace={criteria.MainVtPlace}, MainVtPlaceCode={criteria.MainVtPlaceCode}");
        Console.WriteLine($" ReserveVtPlace={criteria.ReserveVtPlace}, ReserveVtPlaceCode={criteria.ReserveVtPlaceCode}");

        Console.WriteLine($" HasDFZ={criteria.HasDFZ}, DFZEnabled={criteria.DFZEnabled}");
        Console.WriteLine($" HasDZL={criteria.HasDZL}, DZLEnabled={criteria.DZLEnabled}");
        Console.WriteLine($" HasDZ={criteria.HasDZ}, DZEnabled={criteria.DZEnabled}");

        Console.WriteLine($" NeedDisableUpaskReceivers={criteria.NeedDisableUpaskReceivers}");
        Console.WriteLine($" NeedDisconnectLineCTFromDZO={criteria.NeedDisconnectLineCTFromDZO}");
        Console.WriteLine($" CtRemainsEnergizedOnThisSide={criteria.CtRemainsEnergizedOnThisSide}");

        // 5) Реестр операций (ActionOperationRegistry требует IEnumerable<IOperation>)
        var operations = new IOperation[]
        {
            new DfzFieldClosingOperation(),
            new DzlFieldClosingOperation(),
            new DzFieldClosingOperation(),
            new OapvOperation(),
            new TapvOperation(),
            new UpaskReceiversWithdrawalOperation(),
            new LineVtToReserveVoltageCircuitsTransferOperation(),
            new DisconnectLineCtFromDzoOperation(),
            new MtzoShinovkaAtoBOperation(),
            new BusbarVtToReserveBusVtVoltageCircuitsTransferOperation(),

            new DfzNoFieldClosingOperation(),
            new DzNoFieldClosingOperation(),
            new OapvNoFieldClosingOperation(),
            new TapvNoFieldClosingOperation(),

            new DfzSingleSideWithdrawalOperation(),
        };

        IActionOperationRegistry registry = new ActionOperationRegistry(operations);
        var generator = new InstructionGenerator(registry);

        Console.WriteLine();
        Console.WriteLine("=== Generate ===");
        var instructions = generator.Generate(criteria);
        PrintResult(instructions);

        Console.WriteLine();
        Console.WriteLine("=== End ===");
        Console.ReadKey();
    }

    /// <summary>
    /// Гарантирует наличие минимального набора данных, достаточного для чтения DeviceParamsSnapshot.
    /// ВНИМАНИЕ: метод используется ТОЛЬКО при VKR_IT_CONSOLETEST_SEED=1.
    /// При повторном запуске нормализует place/place_code к каноническому виду,
    /// чтобы place всегда оставался русским (UI/вывод), а place_code — кодом (алгоритмы).
    /// </summary>
    private static async Task<long> EnsureMinimalDeviceWithFunctionsAsync(VkrItDbContext db)
    {
        var lineType = await db.ObjectTypes.FirstOrDefaultAsync(x => x.Code == "LINE");
        if (lineType is null)
        {
            lineType = new ObjectType { Code = "LINE", Name = "Линия" };
            db.ObjectTypes.Add(lineType);
            await db.SaveChangesAsync();
        }

        var substation = await db.Substations.FirstOrDefaultAsync(x => x.DispatchName == "ПС 500 кВ Тестовая");
        if (substation is null)
        {
            substation = new Substation { DispatchName = "ПС 500 кВ Тестовая" };
            db.Substations.Add(substation);
            await db.SaveChangesAsync();
        }

        var obj = await db.Objects.FirstOrDefaultAsync(x => x.Uid == "VL500_001");
        if (obj is null)
        {
            obj = new ObjectTable
            {
                ObjectTypeId = lineType.ObjectTypeId,
                Uid = "VL500_001",
                DispatchName = "ВЛ 500 кВ №1",
                IsActive = true,
            };
            db.Objects.Add(obj);
            await db.SaveChangesAsync();
        }

        var device = await db.Devices.FirstOrDefaultAsync(x => x.LineEndId == obj.ObjectId && x.Name == "Устройство РЗА 1");
        if (device is null)
        {
            device = new Device
            {
                LineEndId = obj.ObjectId,
                Name = "Устройство РЗА 1",

                // Технологические параметры устройства (как в целевой архитектуре).
                // Здесь задаём значения только для первичного наполнения.
                VtSwitchTrue = true,
                DzoSwitchTrue = true,
                UpaskSwitchTrue = true,
                FieldClosingAllowed = true,

                // Новый флаг — задаём при создании (не трогаем существующие значения при повторных запусках).
                CtRemainsEnergized = false
            };
            db.Devices.Add(device);
            await db.SaveChangesAsync();
        }

        // CT place: "Сумма токов выключателей линии" -> CT_SUM_BREAKERS
        var ctPlace = await db.CtPlaces.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId);
        if (ctPlace is null)
        {
            ctPlace = new CtPlace
            {
                DeviceId = device.DeviceId,
                Name = "ТТ (место подключения)",
                Place = "Сумма токов выключателей линии",
                PlaceCode = PlaceCodes.Ct.SumBreakers
            };
            db.CtPlaces.Add(ctPlace);
            await db.SaveChangesAsync();
        }
        else
        {
            var changed = false;

            if (!string.Equals(ctPlace.Place, "Сумма токов выключателей линии", StringComparison.Ordinal))
            {
                ctPlace.Place = "Сумма токов выключателей линии";
                changed = true;
            }

            if (!string.Equals(ctPlace.PlaceCode, PlaceCodes.Ct.SumBreakers, StringComparison.Ordinal))
            {
                ctPlace.PlaceCode = PlaceCodes.Ct.SumBreakers;
                changed = true;
            }

            if (changed)
                await db.SaveChangesAsync();
        }

        // VT: основной = линейный, резервный = шинный
        var vtMain = await db.Vts.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Main);
        var vtReserve = await db.Vts.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && !x.Main);

        if (vtMain is null)
        {
            vtMain = new Vt
            {
                DeviceId = device.DeviceId,
                Main = true,
                Name = "ТН линейный",
                Place = "Линейный ТН",
                PlaceCode = PlaceCodes.Vt.Line
            };
            db.Vts.Add(vtMain);
            await db.SaveChangesAsync();
        }
        else
        {
            var changed = false;

            if (!string.Equals(vtMain.Place, "Линейный ТН", StringComparison.Ordinal))
            {
                vtMain.Place = "Линейный ТН";
                changed = true;
            }

            if (!string.Equals(vtMain.PlaceCode, PlaceCodes.Vt.Line, StringComparison.Ordinal))
            {
                vtMain.PlaceCode = PlaceCodes.Vt.Line;
                changed = true;
            }

            if (changed)
                await db.SaveChangesAsync();
        }

        if (vtReserve is null)
        {
            vtReserve = new Vt
            {
                DeviceId = device.DeviceId,
                Main = false,
                Name = "ТН резервный шинный",
                Place = "Шинный ТН",
                PlaceCode = PlaceCodes.Vt.Bus
            };
            db.Vts.Add(vtReserve);
            await db.SaveChangesAsync();
        }
        else
        {
            var changed = false;

            if (!string.Equals(vtReserve.Place, "Шинный ТН", StringComparison.Ordinal))
            {
                vtReserve.Place = "Шинный ТН";
                changed = true;
            }

            if (!string.Equals(vtReserve.PlaceCode, PlaceCodes.Vt.Bus, StringComparison.Ordinal))
            {
                vtReserve.PlaceCode = PlaceCodes.Vt.Bus;
                changed = true;
            }

            if (changed)
                await db.SaveChangesAsync();
        }

        // Функции
        var dfz = await db.Dfzs.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "DFZ");
        if (dfz is null)
        {
            dfz = new Dfz
            {
                DeviceId = device.DeviceId,
                Code = "DFZ",
                Name = "ДФЗ",
                HazDfz = true,
                State = true
            };
            db.Dfzs.Add(dfz);
            await db.SaveChangesAsync();
        }

        var dzl = await db.Dzls.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "DZL");
        if (dzl is null)
        {
            dzl = new Dzl
            {
                DeviceId = device.DeviceId,
                Code = "DZL",
                Name = "ДЗЛ",
                HazDzl = true,
                State = true
            };
            db.Dzls.Add(dzl);
            await db.SaveChangesAsync();
        }

        var dz = await db.Dzs.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "DZ");
        if (dz is null)
        {
            dz = new Dz
            {
                DeviceId = device.DeviceId,
                Code = "DZ",
                Name = "ДЗ",
                HazDz = true,
                State = true
            };
            db.Dzs.Add(dz);
            await db.SaveChangesAsync();
        }

        var oapv = await db.Oapvs.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "OAPV");
        if (oapv is null)
        {
            oapv = new Oapv
            {
                DeviceId = device.DeviceId,
                Code = "OAPV",
                Name = "ОАПВ",
                SwitchOff = false,
                State = true
            };
            db.Oapvs.Add(oapv);
            await db.SaveChangesAsync();
        }

        var tapv = await db.Tapvs.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "TAPV");
        if (tapv is null)
        {
            tapv = new Tapv
            {
                DeviceId = device.DeviceId,
                Code = "TAPV",
                Name = "ТАПВ",
                SwitchOff = false,
                State = true
            };
            db.Tapvs.Add(tapv);
            await db.SaveChangesAsync();
        }

        // МТЗ ошиновки (опционально)
        var mtzBusbar = await db.MtzBusbars.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId);
        if (mtzBusbar is null)
        {
            mtzBusbar = new MtzBusbar
            {
                DeviceId = device.DeviceId,
                Code = "MTZ_BUS",
                Name = "МТЗ ошиновки",
                State = true
            };
            db.MtzBusbars.Add(mtzBusbar);
            await db.SaveChangesAsync();
        }

        return device.DeviceId;
    }

    /// <summary>
    /// Печатает список сформированных указаний в консоль.
    /// </summary>
    private static void PrintResult(System.Collections.Generic.IReadOnlyList<string> instructions)
    {
        if (instructions.Count == 0)
        {
            Console.WriteLine("Результат: операций не требуется");
            return;
        }

        Console.WriteLine("Результат:");
        foreach (var instruction in instructions)
            Console.WriteLine($" - {instruction}");
    }
}
