using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AirlineManager.Shared.Data
{
    public partial class Person
    {
        public Person()
        {
            CertifiedMechanics = new HashSet<CertifiedMechanic>();
            CertifiedPilots = new HashSet<CertifiedPilot>();
        }

        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly StartDate { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly? EndDate { get; set; }
        public string? Email { get; set; }
        public bool EmailPreference { get; set; }

        public virtual ICollection<CertifiedMechanic> CertifiedMechanics { get; set; }
        public virtual ICollection<CertifiedPilot> CertifiedPilots { get; set; }
    }
}
