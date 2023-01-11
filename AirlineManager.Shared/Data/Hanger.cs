using System;
using System.Collections.Generic;

namespace AirlineManager.Shared.Data;

public partial class Hanger
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int HangerType { get; set; }

    public virtual HangerType HangerTypeNavigation { get; set; } = null!;

    public virtual ICollection<Maintenance> Maintenances { get; } = new List<Maintenance>();
}
