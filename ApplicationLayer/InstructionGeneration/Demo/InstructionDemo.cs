using System.Diagnostics;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Generation;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Demo;

/// <summary>
/// Provides a demo runner for instruction generation.
/// </summary>
public static class InstructionDemo
{
    /// <summary>
    /// Runs the demo with in-memory criteria and writes the output to debug and console.
    /// </summary>
    public static void Run()
    {
        var criteria = new List<LineOperationCriteria>
        {
            new("VL-101", SideOfLine.Begin, 1, ActionCode.VlOutFieldClosing)
            {
                DeviceName = "DFZ-Device-1",
                HasDFZ = true,
                DFZEnabled = true,
                DFZConnectedToLineCT = true,
                DFZConnectedToLineVT = true,
                NeedSwitchLineVTToReserve = true,
            },
            new("VL-101", SideOfLine.Begin, 2, ActionCode.VlOutFieldClosing)
            {
                DeviceName = "DZ-Device-2",
                HasDZ = true,
                DZEnabled = true,
                DZConnectedToLineVT = true,
                NeedSwitchBusVTToReserve = true,
            },
            new("VL-101", SideOfLine.End, 3, ActionCode.VlOutNoFieldClosing)
            {
                DeviceName = "OAPV-Device-3",
                HasOAPV = true,
                OAPVEnabled = true,
                BothCbctReverseSide = true,
            }
        };

        var provider = new InMemoryLineCriteriaProvider(criteria);
        var generator = new InstructionGenerator(ActionCode.VlOutFieldClosing);
        var result = generator.Generate(provider.GetCriteria(ActionCode.VlOutFieldClosing));

        foreach (var item in result.Items)
        {
            var message = $"[{item.Code}] ({item.Side}) Device={item.DeviceId} -> {item.Text}";
            Debug.WriteLine(message);
            Console.WriteLine(message);
        }
    }
}
