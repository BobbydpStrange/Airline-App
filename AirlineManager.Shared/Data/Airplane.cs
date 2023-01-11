using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class Airplane
{
    public int Id { get; set; }

    public int TailNumber { get; set; }

    public int AirplaneTypeId { get; set; }

    public virtual AirplaneType AirplaneType { get; set; } = null!;

    public virtual ICollection<Flight> Flights { get; } = new List<Flight>();

    public virtual ICollection<Maintenance> Maintenances { get; } = new List<Maintenance>();
}
