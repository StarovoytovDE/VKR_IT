using System.Collections.Generic;

namespace Domain.Entities;

public sealed class ObjectTable
{
    public long ObjectId { get; set; }
    public long ObjectTypeId { get; set; }
    public string Uid { get; set; } = string.Empty;
    public string DispatchName { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ObjectType ObjectType { get; set; } = null!;
    public ICollection<ObjectParamValue> ObjectParamValues { get; set; } = new List<ObjectParamValue>();
    public ICollection<InstructionRequest> InstructionRequests { get; set; } = new List<InstructionRequest>();
    /// <summary>
    /// Связи, где объект выступает линией и связан с устройствами РЗА.
    /// </summary>
    public ICollection<LineRzaDeviceLink> LineDevices { get; set; } = new List<LineRzaDeviceLink>();
    /// <summary>
    /// Связи, где объект выступает устройством РЗА, подключенным к линии.
    /// </summary>
    public ICollection<LineRzaDeviceLink> DeviceLines { get; set; } = new List<LineRzaDeviceLink>();
}
