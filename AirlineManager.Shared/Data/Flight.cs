using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class Flight
{
    public int Id { get; set; }

    public int OfferedFlightId { get; set; }

    public int PilotId { get; set; }

    public int AirplaneId { get; set; }

    public DateTime EstDeparture { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public bool? Cancelled { get; set; }

    public virtual Airplane Airplane { get; set; } = null!;

    public virtual OfferedFlight OfferedFlight { get; set; } = null!;

    public virtual CertifiedPilot Pilot { get; set; } = null!;
}
