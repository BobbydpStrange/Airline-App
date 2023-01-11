using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class AirplaneType
{
    public int Id { get; set; }

    public string Manufacturer { get; set; } = null!;

    public int? BusinessCapacity { get; set; }

    public int? CoachCapacity { get; set; }

    public int? FirstClassCapacity { get; set; }

    public virtual ICollection<Airplane> Airplanes { get; } = new List<Airplane>();

    public virtual ICollection<CertifiedMechanic> CertifiedMechanics { get; } = new List<CertifiedMechanic>();

    public virtual ICollection<CertifiedPilot> CertifiedPilots { get; } = new List<CertifiedPilot>();
}
