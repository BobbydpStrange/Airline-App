using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class HangerType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Hanger> Hangers { get; } = new List<Hanger>();
}
