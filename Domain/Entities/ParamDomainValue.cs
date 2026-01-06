namespace Domain.Entities;

public sealed class ParamDomainValue
{
    public long ParamDomainId { get; set; }
    public int ValueCode { get; set; }
    public string ValueName { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public ParamDomain ParamDomain { get; set; } = null!;
}
