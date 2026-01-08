namespace ApplicationLayer.InstructionGeneration.Models;

public sealed class InstructionItem
{
    public InstructionItem(string code, string title, string text, int order, InstructionItemType type = InstructionItemType.Info)
    {
        Code = code;
        Title = title;
        Text = text;
        Order = order;
        Type = type;
    }

    public string Code { get; }
    public string Title { get; }
    public string Text { get; }
    public int Order { get; }
    public InstructionItemType Type { get; }
}
