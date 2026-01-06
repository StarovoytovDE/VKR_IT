using System.Collections.Generic;

namespace Domain.Entities;

public sealed class ParamDomain
{
    public long ParamDomainId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public ICollection<ParamDomainValue> ParamDomainValues { get; set; } = new List<ParamDomainValue>();
    public ICollection<ParamDefinition> ParamDefinitions { get; set; } = new List<ParamDefinition>();
}
