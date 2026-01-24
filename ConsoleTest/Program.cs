using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using ApplicationLayer.InstructionGeneration.Requests;
using Domain.Entities;
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

        var deviceId = await EnsureMinimalDeviceWithFunctionsAsync(db);

        var reader = new EfCoreDeviceParamsReader(db);
        var snapshot = await reader.ReadAsync(deviceId, CancellationToken.None);

        Console.WriteLine();
        Console.WriteLine($"Snapshot: DeviceId={snapshot.DeviceId}, ObjectId={snapshot.ObjectId}, Name={snapshot.DeviceName}");
        Console.WriteLine($"Snapshot: VtSwitchTrue={snapshot.VtSwitchTrue}");
        Console.WriteLine($"Snapshot: VT Main='{snapshot.Vts.Main.Name}', Place='{snapshot.Vts.Main.Place}'");
        Console.WriteLine($"Snapshot: VT Reserve='{snapshot.Vts.Reserve.Name}', Place='{snapshot.Vts.Reserve.Place}'");
        Console.WriteLine($"Snapshot: CT Place='{snapshot.CtPlace.Place}'");

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

        var builder = new LineOperationCriteriaBuilder();

        var deviceObjectId = checked((int)snapshot.ObjectId);
        var criteria = builder.Build(request, deviceObjectId, snapshot);

        // ВРЕМЕННО: технологические/паспортные флаги, которые ещё не заведены в БД как отдельные поля/таблицы.
        // ВАЖНО: DeviceConnectedToLineCT теперь вычисляется builder-ом из snapshot.CtPlace.Place
        // и не должен задаваться вручную.
        criteria = criteria with
        {
            IsFieldClosingAllowed = true,
            NeedDisableUpaskReceivers = true,
            NeedDisconnectLineCTFromDZO = true,
            NeedMtzoShinovkaAtoB = false,

            IsOnlyFunctionInDevice = false,
            HasMtzoShinovka = false,
            BothLineBreakerCTsOnSubstationSide = false
        };

        Console.WriteLine();
        Console.WriteLine("Criteria (summary):");
        Console.WriteLine($" ActionCode={criteria.ActionCode}, Side={criteria.Side}, LineCode={criteria.LineCode}");
        Console.WriteLine($" IsFieldClosingAllowed={criteria.IsFieldClosingAllowed}");
        Console.WriteLine($" HasDFZ={criteria.HasDFZ}, DFZEnabled={criteria.DFZEnabled}, DeviceConnectedToLineCT={criteria.DeviceConnectedToLineCT}, DFZConnectedToLineVT={criteria.DFZConnectedToLineVT}");
        Console.WriteLine($" HasDZL={criteria.HasDZL}, DZLEnabled={criteria.DZLEnabled}");
        Console.WriteLine($" HasDZ={criteria.HasDZ}, DZEnabled={criteria.DZEnabled}, DZConnectedToLineVT={criteria.DZConnectedToLineVT}");
        Console.WriteLine($" NeedSwitchVTToReserve={criteria.NeedSwitchVTToReserve}");
        Console.WriteLine($" NeedSwitchFromLineVTToReserve={criteria.NeedSwitchFromLineVTToReserve}");
        Console.WriteLine($" NeedDisableUpaskReceivers={criteria.NeedDisableUpaskReceivers}");
        Console.WriteLine($" NeedDisconnectLineCTFromDZO={criteria.NeedDisconnectLineCTFromDZO}");

        var operations = new IOperation[]
        {
            new DfzFieldClosingOperation(),
            new DzlFieldClosingOperation(),
            new DzFieldClosingOperation(),
            new UpaskReceiversWithdrawalOperation(),
            new LineVtToReserveVoltageCircuitsTransferOperation(),
            new DisconnectLineCtFromDzoOperation(),
            new MtzoShinovkaAtoBOperation(),
            new OapvOperation(),
            new TapvOperation(),

            new DfzNoFieldClosingOperation(),
            new DzNoFieldClosingOperation(),
            new OapvNoFieldClosingOperation(),
            new TapvNoFieldClosingOperation(),

            new BusbarVtToReserveBusVtVoltageCircuitsTransferOperation(),

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
                SubstationId = substation.SubstationId
            };
            db.Objects.Add(obj);
            await db.SaveChangesAsync();
        }

        var device = await db.Devices.FirstOrDefaultAsync(x => x.ObjectId == obj.ObjectId && x.Name == "Устройство РЗА 1");
        if (device is null)
        {
            device = new Device
            {
                ObjectId = obj.ObjectId,
                Name = "Устройство РЗА 1",
                VtSwitchTrue = true
            };
            db.Devices.Add(device);
            await db.SaveChangesAsync();
        }
        else if (!device.VtSwitchTrue)
        {
            device.VtSwitchTrue = true;
            await db.SaveChangesAsync();
        }

        // CT place — по утверждённому словарю.
        // Для того, чтобы ветка ДФЗ работала как "не линейный ТТ",
        // выставляем "Сумма токов выключателей линии".
        var ctPlace = await db.CtPlaces.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId);
        if (ctPlace is null)
        {
            ctPlace = new CtPlace
            {
                DeviceId = device.DeviceId,
                Name = "ТТ (место подключения)",
                Place = "Сумма токов выключателей линии"
            };
            db.CtPlaces.Add(ctPlace);
            await db.SaveChangesAsync();
        }
        else if (ctPlace.Place != "Сумма токов выключателей линии")
        {
            ctPlace.Place = "Сумма токов выключателей линии";
            await db.SaveChangesAsync();
        }

        // VT: по твоей модели Place:
        // "Линейный ТН", "Шинный ТН", "ТН ошиновки"
        var vtMain = await db.Vts.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Main);
        var vtReserve = await db.Vts.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && !x.Main);

        if (vtMain is null)
        {
            vtMain = new Vt
            {
                DeviceId = device.DeviceId,
                Main = true,
                Name = "ТН линейный",
                Place = "Линейный ТН"
            };
            db.Vts.Add(vtMain);
            await db.SaveChangesAsync();
        }
        else if (vtMain.Place != "Линейный ТН")
        {
            vtMain.Place = "Линейный ТН";
            await db.SaveChangesAsync();
        }

        if (vtReserve is null)
        {
            vtReserve = new Vt
            {
                DeviceId = device.DeviceId,
                Main = false,
                Name = "ТН резервный шинный",
                Place = "Шинный ТН"
            };
            db.Vts.Add(vtReserve);
            await db.SaveChangesAsync();
        }
        else if (vtReserve.Place == "Линейный ТН")
        {
            vtReserve.Place = "Шинный ТН";
            await db.SaveChangesAsync();
        }

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
