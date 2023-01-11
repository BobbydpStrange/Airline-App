using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AirlineManager.Shared.Data;

public partial class CertifiedPilot
{
    public int Id { get; set; }

    public int PeopleId { get; set; }

    public int AirplaneTypeId { get; set; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly CertDate { get; set; }

    public int Expiremonths { get; set; }

    public virtual AirplaneType AirplaneType { get; set; } = null!;

    public virtual ICollection<Flight> Flights { get; } = new List<Flight>();

    public virtual Person People { get; set; } = null!;
}
