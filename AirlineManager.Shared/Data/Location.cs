using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class Location
{
    public int Id { get; set; }

    public string City { get; set; } = null!;

    public virtual ICollection<Airport> Airports { get; } = new List<Airport>();
}
