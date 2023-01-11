using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class OfferedFlight
{
    public int Id { get; set; }

    public int FromAirport { get; set; }

    public int ToAirport { get; set; }

    public int EstFlightMinutes { get; set; }

    public virtual ICollection<Flight> Flights { get; } = new List<Flight>();

    public virtual Airport FromAirportNavigation { get; set; } = null!;

    public virtual Airport ToAirportNavigation { get; set; } = null!;
}
