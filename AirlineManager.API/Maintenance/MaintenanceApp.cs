using AirlineManager.API.Repository;
using AirlineManager.Shared.Data;
using Microsoft.AspNetCore.Mvc;

namespace AirlineManager.API.Maintenance
{
    public class MaintenanceApp
    {
        private readonly IDataRepository repo;
        private readonly AirmanEmailService emailService;

        public MaintenanceApp(IDataRepository repo, AirmanEmailService emailService)
        {
            this.repo = repo;
            this.emailService = emailService;
        }

        public IEnumerable<MaintenanceType> GetMaintenanceTypes()
        {
            return repo.GetMaintenanceTypes();
        }

        public async Task UpdateMaintenanceTypesAsync(IEnumerable<MaintenanceType> maintenanceTypes)
        {
            await repo.UpdateMaintenanceTypesAsync(maintenanceTypes);
        }

        public async Task UpdateMaintenanceTypeAsync(MaintenanceTypeEdit maintenanceType)
        {
            await repo.UpdateMaintenanceTypeAsync(maintenanceType);
        }

        public async Task DeleteMaintenanceTypeAsync(int id)
        {
            await repo.DeleteMaintenanceTypeAsync(id);
        }

        public IEnumerable<PlanesDueForMaintenance> GetPlanesDue()
        {
            return repo.GetPlanesDueForMaintenance();
        }

        public async Task<IEnumerable<Shared.Data.Maintenance>> GetMaintenanceAsync()
        {
            return await repo.GetMaintenance();
        }

        public async Task<IEnumerable<CertifiedMechanic>> GetAvailableMechanicsForTimeRange(DateTime start, DateTime end)
        {
            var mechs = await repo.GetAvailableMechanicsForTimeRangeAsync(start, end);
            return mechs;
        }

        public async Task<IEnumerable<Airplane>> GetAirplanes()
        {
            return await repo.GetAirplanes();
        }

        internal async Task<IEnumerable<Hanger>> GetHangers()
        {
            return await repo.GetHangers();
        }

        internal async Task<bool> AddMaintenance(NewMaintenance newMaintenance)
        {
            var person = await repo.GetWorkerDetails(newMaintenance.MechanicId);
            var success = await repo.AddMaintenance(newMaintenance);
            if (success)
            {
                if (person!= null)
                {
                    if (person.EmailPreference)
                    {
                        await emailService.NotifyScheduleChange(person.Email);
                    }
                }
            }
            return success;
        }

        internal async Task<Person> GetPersonByEmail(string email)
        {
            return await repo.GetPersonByEmail(email);
        }

        internal async Task<bool> UpdatePerson(Person person)
        {
            return await repo.UpdatePerson(person);
        }
    }
}
