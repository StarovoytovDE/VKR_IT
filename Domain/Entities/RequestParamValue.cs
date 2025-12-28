using System.Text.Json;

namespace Domain.Entities;

public sealed class RequestParamValue
{
    public long InstructionRequestId { get; set; }
    public long ParamDefinitionId { get; set; }
    public string ValueOrigin { get; set; } = string.Empty;
    public bool? ValueBool { get; set; }
    public int? ValueIntEnum { get; set; }
    public decimal? ValueDecimal { get; set; }
    public string? ValueText { get; set; }
    public JsonDocument? ValueJson { get; set; }

    public InstructionRequest InstructionRequest { get; set; } = null!;
    public ParamDefinition ParamDefinition { get; set; } = null!;
}
