using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class PlanesDueForMaintenance
{
    public int? Planeid { get; set; }

    public int? TailNumber { get; set; }

    public string? Manufacturer { get; set; }

    public int? Mainttypeid { get; set; }

    public string? MaintType { get; set; }

    public int? EstTimeMinutes { get; set; }

    public bool? Isemergency { get; set; }

    public DateTime? LastServiced { get; set; }

    public int? FlightsSinceLastService { get; set; }

    public int? FlightsBeforeNeeded { get; set; }
}
