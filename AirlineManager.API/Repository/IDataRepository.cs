using AirlineManager.Shared.Data;

namespace AirlineManager.API.Repository
{
    public interface IDataRepository
    {
        Task DeleteMaintenanceTypeAsync(int id);
        Task <IEnumerable<Shared.Data.Maintenance>> GetMaintenance();
        IEnumerable<MaintenanceType> GetMaintenanceTypes();
        IEnumerable<PlanesDueForMaintenance> GetPlanesDueForMaintenance();
        Task UpdateMaintenanceTypeAsync(MaintenanceTypeEdit maintenanceType);
        Task UpdateMaintenanceTypesAsync(IEnumerable<MaintenanceType> maintenanceTypes);

        Task<IEnumerable<CertifiedMechanic>> GetAvailableMechanicsForTimeRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<Airplane>> GetAirplanes();
        Task<IEnumerable<Hanger>> GetHangers();
        Task<bool> AddMaintenance(NewMaintenance newMaintenance);
        Task<Person> GetWorkerDetails(int mechanicId);
        Task<Person> GetPersonByEmail(string email);
        Task<bool> UpdatePerson(Person person);
    }
}
