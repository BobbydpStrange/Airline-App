using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class MaintenanceCertification
{
    public int Id { get; set; }

    public string CertificationType { get; set; } = null!;

    public int ExpirationMonths { get; set; }

    public virtual ICollection<CertifiedMechanic> CertifiedMechanics { get; } = new List<CertifiedMechanic>();

    public virtual ICollection<MaintenanceType> MaintenanceTypes { get; } = new List<MaintenanceType>();
}
