using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class Maintenance
{
    public int Id { get; set; }

    public int MaintenanceTypeId { get; set; }

    public int AirplaneId { get; set; }

    public int MechanicId { get; set; }

    public int HangerId { get; set; }

    public DateTime Assigned { get; set; }

    public DateTime? Completed { get; set; }

    public virtual Airplane Airplane { get; set; } = null!;

    public virtual Hanger Hanger { get; set; } = null!;

    public virtual MaintenanceType MaintenanceType { get; set; } = null!;

    public virtual CertifiedMechanic Mechanic { get; set; } = null!;
}
