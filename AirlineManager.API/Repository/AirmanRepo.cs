using AirlineManager.API.Data;
using AirlineManager.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Text.Json;

namespace AirlineManager.API.Repository
{
    public class AirmanRepo : IDataRepository
    {
        private readonly AirmanDbContext context;
        private readonly ILogger<AirmanDbContext> logger;

        public AirmanRepo(AirmanDbContext context, ILogger<AirmanDbContext> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task DeleteMaintenanceTypeAsync(int id)
        {
            logger.LogWarning("This cannont be undone, but its too late to stop it now...");
            var type = context.MaintenanceTypes.FirstOrDefault(x => x.Id == id);
            if (type != null)
            {
                context.MaintenanceTypes.Remove(type);
                await context.SaveChangesAsync();
            }
        }

        public IEnumerable<MaintenanceType> GetMaintenanceTypes()
        {
            logger.LogDebug("Getting Maintenance Types");
            return context.MaintenanceTypes;
        }

        public IEnumerable<PlanesDueForMaintenance> GetPlanesDueForMaintenance()
        {
            logger.LogDebug("Planes Due for Maintenance");
            logger.LogWarning("This request may take a while");
            return context.PlanesDueForMaintenances;
        }

        public async Task UpdateMaintenanceTypeAsync(MaintenanceTypeEdit maintenanceType)
        {
            var type = context.MaintenanceTypes.FirstOrDefault(x => x.Id== maintenanceType.Id);
            type.MaintType = maintenanceType.MaintType;
            type.MaintFlightPolicy= maintenanceType.MaintFlightPolicy;
            await context.SaveChangesAsync();
        }

        public async Task UpdateMaintenanceTypesAsync(IEnumerable<MaintenanceType> maintenanceTypes)
        {
            logger.LogInformation("Updating maintenance types...");
            foreach(var type in maintenanceTypes)
            {
                context.MaintenanceTypes.Update(type);
            }
            await context.SaveChangesAsync();
            logger.LogInformation("Done updating maintenance types.");
        }

        public async Task<IEnumerable<Shared.Data.Maintenance>> GetMaintenance()
        {
            logger.LogWarning("This query might take a while");
            return await context.Maintenances
                .Include(m => m.Mechanic).ThenInclude(m => m.People)
                .Include(m => m.Airplane)
                .Include(m => m.MaintenanceType)
                .Include(m => m.Hanger).Where(m => m.Completed == null).ToListAsync();
        }

        public async Task<IEnumerable<CertifiedMechanic>> GetAvailableMechanicsForTimeRangeAsync(DateTime start, DateTime end)
        {
            logger.LogDebug("Getting Certified Mechanics from the database.");
            logger.LogWarning("This query might take a while");
            var availablemechs = (await context.GetAvailableMechanics(start, end).ToListAsync())
                .Select(cm =>
                {
                    var mech = context.CertifiedMechanics
                        .Where(m => m.Id == cm.CertifiedMechId)
                        .Include(m => m.Cert)
                        .Include(m => m.People)
                        .Select(cm => new CertifiedMechanic()
                        {
                            // I had to do this select to get only what I needed on the joins,
                            // if I included the whole objects, it would run out of memory.
                            Id = cm.Id,
                            AirplaneType = new AirplaneType() { Manufacturer = cm.AirplaneType.Manufacturer },
                            CertDate = cm.CertDate,
                            AirplaneTypeId = cm.AirplaneTypeId,
                            Cert = new MaintenanceCertification() { ExpirationMonths = cm.Cert.ExpirationMonths },
                            People = cm.People,
                            PeopleId = cm.PeopleId,
                            CertId = cm.CertId
                        })
                        .FirstOrDefault();
                    var airplanetype = context.AirplaneTypes
                        .Where(at => at.Id == cm.CanFixPlaneType)
                        .FirstOrDefault();

                    return new CertifiedMechanic()
                    {
                        Id = cm.CertifiedMechId,
                        AirplaneType = new AirplaneType() { Manufacturer = airplanetype.Manufacturer },
                        CertDate = mech.CertDate,
                        AirplaneTypeId = cm.CanFixPlaneType,
                        Cert = new MaintenanceCertification() { ExpirationMonths = mech.Cert.ExpirationMonths },
                        People = mech.People,
                        PeopleId = mech.PeopleId,
                        CertId = mech.CertId
                    };
                });
            logger.LogDebug("Retrieved Certified Mechanics");
            
            return availablemechs;
        }

        public async Task<IEnumerable<Airplane>> GetAirplanes()
        {
            return await context.Airplanes.ToListAsync();
        }

        public async Task<IEnumerable<Hanger>> GetHangers()
        {
            return await context.Hangers
            .ToListAsync();
        }

        public async Task<bool> AddMaintenance(NewMaintenance newMaintenance)
        {
            try
            {
                await context.Maintenances.AddAsync(new Shared.Data.Maintenance() { 
                    AirplaneId= newMaintenance.AirplaneId,
                    Assigned = newMaintenance.Assigned,
                    HangerId= newMaintenance.HangerId,
                    MaintenanceTypeId= newMaintenance.MaintenanceTypeId,
                    MechanicId= newMaintenance.MechanicId
                });
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogWarning("The maintenance could not be scheduled", ex);
                return false;
            }

        }

        public async Task<Person> GetWorkerDetails(int mechanicId)
        {
            var mech = context.CertifiedMechanics.Where(cm => cm.Id == mechanicId).Include(cm => cm.People).First().People;
            if (mech == null)
            {
                logger.LogError("There is no person for that mechanic id");
            }
            return mech;
        }

        public Task<Person> GetPersonByEmail(string email)
        {
            var person = context.People.Where(p => p.Email == email).FirstOrDefaultAsync();
            if (person == null)
            {
                logger.LogError($"Getting person by email @{email} returned null");
            }
            return person;
        }

        public async Task<bool> UpdatePerson(Person person)
        {
            logger.LogInformation("The person is being updated.");
            var result = context.People.Update(person);
            var numofchanges = await context.SaveChangesAsync();
            if (numofchanges < 1)
                logger.LogError("The person was not updated.");
            logger.LogInformation("The person was updated");
            return true;
        }
    }
}
