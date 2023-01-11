using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class FlightTimeTemplate
{
    public int Id { get; set; }

    public TimeOnly? ScheduledTime { get; set; }
}
