using AirlineManager.API.Repository;
using AirlineManager.Shared.Data;

namespace AirlineManager.APITests
{
    public class TestDataService : IDataRepository
    {
        public Task<bool> AddMaintenance(NewMaintenance newMaintenance)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMaintenanceTypeAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Airplane>> GetAirplanes()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CertifiedMechanic>> GetAvailableMechanicsForTimeRangeAsync(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Hanger>> GetHangers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Maintenance> GetMaintenance()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MaintenanceType> GetMaintenanceTypes()
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetPersonByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PlanesDueForMaintenance> GetPlanesDueForMaintenance()
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetWorkerDetails(int mechanicId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMaintenanceTypeAsync(MaintenanceTypeEdit maintenanceType)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMaintenanceTypesAsync(IEnumerable<MaintenanceType> maintenanceTypes)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePerson(Person person)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Maintenance>> IDataRepository.GetMaintenance()
        {
            throw new NotImplementedException();
        }
    }
}