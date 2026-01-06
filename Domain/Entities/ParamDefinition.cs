using System.Collections.Generic;

namespace Domain.Entities;

public sealed class ParamDefinition
{
    public long ParamDefinitionId { get; set; }
    public long? ParamDomainId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public string? Unit { get; set; }
    public string? Description { get; set; }

    public ParamDomain? ParamDomain { get; set; }
    public ICollection<ActionParamRequirement> ActionParamRequirements { get; set; } = [];
    public ICollection<ObjectParamValue> ObjectParamValues { get; set; } = [];
    public ICollection<RequestParamValue> RequestParamValues { get; set; } = [];
}
