using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Generation;
using System.Diagnostics;

namespace ApplicationLayer.InstructionGeneration.Demo;

public static class InstructionDemo
{
    public static void Run()
    {
        var ctx = InstructionContext.CreateSample();

        var generator = new VLOutFieldClosingInstructionGenerator();
        var result = generator.Generate(ctx);

        Debug.WriteLine("=== СФОРМИРОВАННЫЕ УКАЗАНИЯ ===");

        foreach (var item in result.Items)
        {
            Debug.WriteLine(
                $"[{item.Type}] ({item.Order}) {item.Code} — {item.Text}");
        }
    }
}
