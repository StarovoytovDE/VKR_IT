using System.Text.Json;

namespace Domain.Entities;

public sealed class ActionParamRequirement
{
    public long ActionVersionId { get; set; }
    public long ParamDefinitionId { get; set; }
    public bool IsRequired { get; set; }
    public string ValueKind { get; set; } = string.Empty;
    public string? UiPrompt { get; set; }
    public JsonDocument? ValidationRule { get; set; }
    public int SortOrder { get; set; }

    public ActionVersion ActionVersion { get; set; } = null!;
    public ParamDefinition ParamDefinition { get; set; } = null!;
}
