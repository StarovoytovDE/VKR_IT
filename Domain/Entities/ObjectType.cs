using System.Collections.Generic;

namespace Domain.Entities;

public sealed class ObjectType
{
    public long ObjectTypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public ICollection<Object> Objects { get; set; } = new List<Object>();
}
