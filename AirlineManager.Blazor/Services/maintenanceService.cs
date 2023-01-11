using AirlineManager.Blazor.Pages.Maintenance;
using AirlineManager.Blazor.Pages.Maintenance.MaintenanceScheduling;
using AirlineManager.Shared.Data;
using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json;

namespace AirlineManager.Blazor.Services
{
    public class MaintenanceService : IMaintenanceDataService
    {
        private readonly HttpClient client;
        private readonly ILogger<MaintenanceService> logger;
        const string ENDPOINT = "api/maintenance/";

        public MaintenanceService(HttpClient client, ILogger<MaintenanceService> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task <List<MaintenanceType>> GetMaintenanceTypes()
        {
            return await client.GetFromJsonAsync<List<MaintenanceType>>(ENDPOINT + "maintenancetypes");
        }

        public async Task<List<PlanesDueForMaintenance>> GetPlanesDueForMaintenance()
        {
            return await client.GetFromJsonAsync<List<PlanesDueForMaintenance>>(ENDPOINT + "planesdue");
        }

        public async Task UpdateMaintenanceType(MaintenanceTypeEdit maintenanceType)
        {
            await client.PostAsJsonAsync(ENDPOINT + "updateMaintenancetype", maintenanceType);
        }

        public async Task UpdateConfig(IEnumerable<MaintenanceType> maintenanceTypes)
        {
            var res = await client.PostAsJsonAsync(ENDPOINT + "updatemaintenancetypes", maintenanceTypes);
            try
            {
                res.EnsureSuccessStatusCode();
            }
            catch(Exception ex) {
            logger.LogError(ex.Message, ex);
            }
            
        }

        public async Task DeleteMaintenanceType(int id)
        {
            logger.LogInformation($"Deleting maintenance type {id}");
            await client.DeleteAsync($"maintenancetype/{id}");
        }
        public async Task<List<Airplane>> GetAirplanes()
        {
            return await client.GetFromJsonAsync<List<Airplane>>(ENDPOINT + "airplanes");
        }

        public async Task<IEnumerable<CertifiedMechanic>> GetAvailableMechanicsAsync(DateTime scheduledTime, DateTime estCompletion, int maintenanceTypeId, int planeId)
        {
            var res = await client.PostAsJsonAsync<(DateTime, DateTime)>(ENDPOINT + "mechanics", (scheduledTime, estCompletion));
            var allmechs = await res.Content.ReadFromJsonAsync<IEnumerable<CertifiedMechanic>>();
            var planetype = (await GetAirplanes()).Find(a => a.Id == planeId).AirplaneTypeId;
            var requiredCertId = (await GetMaintenanceTypes()).Find(mt=> mt.Id == maintenanceTypeId)?.RequiredCertificationId;
            return allmechs.Where(m => m.CertId == requiredCertId).Where(m => m.AirplaneTypeId == planetype);
        }

        public async Task <List<Maintenance>> GetMaintenance()
        {
            return await client.GetFromJsonAsync<List<Maintenance>>(ENDPOINT + "mechanic/schedule");//or schedule?
        }

        public async Task<List<Hanger>> GetHangersAsync()
        {
            return await client.GetFromJsonAsync<List<Hanger>>(ENDPOINT + "hangers");
        }

        public async Task<bool> AttemptToScheduleMaintenance( NewMaintenance newMaintenance)
        {
            var result = await client.PostAsJsonAsync(ENDPOINT + "schedulenew", newMaintenance);
            return await result.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            Person person = null;
            try
            {
                person = await client.GetFromJsonAsync<Person>(ENDPOINT + "person/" + email);
            }
            catch
            {
                logger.LogWarning("Person by email returned null, there probably is not a person linked to the logged in user.");
            }
            return person;
        }

        public async Task<bool> UpdatePerson(Person person)
        {
            var res = await client.PostAsJsonAsync(ENDPOINT + "person/update", person);
            return res.IsSuccessStatusCode;
        }
    }
}
