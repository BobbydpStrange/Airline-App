using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AirlineManager.Shared.Data;

public partial class CertifiedMechanic
{
    public int Id { get; set; }

    public int PeopleId { get; set; }

    public int CertId { get; set; }

    public int AirplaneTypeId { get; set; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly CertDate { get; set; }

    public virtual AirplaneType AirplaneType { get; set; } = null!;

    public virtual MaintenanceCertification Cert { get; set; } = null!;

    public virtual ICollection<Maintenance> Maintenances { get; } = new List<Maintenance>();

    public virtual Person People { get; set; } = null!;
}
