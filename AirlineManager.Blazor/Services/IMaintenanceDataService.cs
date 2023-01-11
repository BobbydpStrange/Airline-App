using AirlineManager.Shared.Data;

namespace AirlineManager.Blazor.Services
{
	public interface IMaintenanceDataService
	{
		public Task<List<MaintenanceType>> GetMaintenanceTypes();

		public Task<List<PlanesDueForMaintenance>> GetPlanesDueForMaintenance();

		public Task UpdateMaintenanceType(MaintenanceTypeEdit maintenanceType);

		public Task UpdateConfig(IEnumerable<MaintenanceType> maintenanceTypes);

		public Task DeleteMaintenanceType(int id);
		public Task<List<Airplane>> GetAirplanes();

		public Task<IEnumerable<CertifiedMechanic>> GetAvailableMechanicsAsync(DateTime scheduledTime, DateTime estCompletion, int maintenanceTypeId, int planeId);

		public Task<List<Maintenance>> GetMaintenance();

		public Task<List<Hanger>> GetHangersAsync();

		public Task<bool> AttemptToScheduleMaintenance(NewMaintenance newMaintenance);

		public Task<Person> GetPersonByEmail(string email);

		public Task<bool> UpdatePerson(Person person);	
	}
}
