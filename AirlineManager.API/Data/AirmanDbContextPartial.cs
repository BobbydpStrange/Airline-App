using AirlineManager.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace AirlineManager.API.Data
{
    public partial class AirmanDbContext
    {
        public IQueryable<AvalailableMechanics> GetAvailableMechanics(DateTime start, DateTime end)
        {
            return Set<AvalailableMechanics>().FromSqlInterpolated($"select maintenance_type, mech_name, certified_mech_id , can_fix_plane_type from airman.getavailablecertifiedmechanicsfortimerange({start}, {end})");

        }
    }

    public class AvalailableMechanics
    {
        public int CertifiedMechId { get; set;}

        public string MechName { get; set; }
        public int CanFixPlaneType { get; set; }
        public int MaintenanceType { get; set; }
    }
}
