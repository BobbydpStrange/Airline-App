using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class Airport
{
    public int Id { get; set; }

    public string AirportName { get; set; } = null!;

    public int LocationId { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual ICollection<OfferedFlight> OfferedFlightFromAirportNavigations { get; } = new List<OfferedFlight>();

    public virtual ICollection<OfferedFlight> OfferedFlightToAirportNavigations { get; } = new List<OfferedFlight>();
}
