using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirlineManager.Shared.Data;

public partial class MaintenanceType
{
    public int Id { get; set; }

    public string MaintType { get; set; } = null!;

    public int RequiredCertificationId { get; set; }

    public bool Isemergency { get; set; }

    public int EstTimeMinutes { get; set; }

    [Range(1, 1000000, ErrorMessage = "Must enter a valid number of flights")]
    public int MaintFlightPolicy { get; set; }

    public virtual ICollection<Maintenance> Maintenances { get; } = new List<Maintenance>();

    public virtual MaintenanceCertification RequiredCertification { get; set; } = null!;
}

