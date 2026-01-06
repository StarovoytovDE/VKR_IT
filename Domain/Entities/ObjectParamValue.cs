using System;
using System.Text.Json;

namespace Domain.Entities;

public sealed class ObjectParamValue
{
    public long ObjectId { get; set; }
    public long ParamDefinitionId { get; set; }
    public long UpdatedByUserId { get; set; }
    public bool? ValueBool { get; set; }
    public int? ValueIntEnum { get; set; }
    public decimal? ValueDecimal { get; set; }
    public string? ValueText { get; set; }
    public JsonDocument? ValueJson { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public ObjectTable Object { get; set; } = null!;
    public ParamDefinition ParamDefinition { get; set; } = null!;
    public AppUser UpdatedByUser { get; set; } = null!;
}
