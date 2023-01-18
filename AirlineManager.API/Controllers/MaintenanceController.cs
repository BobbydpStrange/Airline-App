using AirlineManager.API.Maintenance;
using AirlineManager.API.Repository;
using AirlineManager.Shared.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AirlineManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaintenanceController : ControllerBase
    {
        private readonly ILogger<MaintenanceApp> _logger;
        private readonly MaintenanceApp maintenanceApp;
        private readonly ProblemDetails problemDetail;

        public MaintenanceController(ILogger<MaintenanceApp> logger, MaintenanceApp maintenanceApp)
        {
            _logger = logger;
            this.maintenanceApp = maintenanceApp;
            problemDetail = new ProblemDetails();
        }
        // GET: api/<MaintenanceController>
        [HttpGet ("maintenancetypes")]
        public IEnumerable<MaintenanceType> GetMaintenanceTypes()
        {
            var s = problemDetail.Status;
            _logger.LogInformation("Getting maintenance types in API, with status code as: {0}", s);

            return maintenanceApp.GetMaintenanceTypes();
            
        }

        [HttpPost("[action]")]
        public async Task UpdateMaintenanceTypesAsync(IEnumerable<MaintenanceType> maintenanceTypes)
        {
            var s = problemDetail.Instance;
            _logger.LogInformation($"UpdateMaintenanceTypes Instance: {s}");
            await maintenanceApp.UpdateMaintenanceTypesAsync(maintenanceTypes);
        }

        [HttpPost("[action]")]
        public async Task UpdateMaintenanceTypeAsync(MaintenanceTypeEdit maintenanceType)
        {
            var s = problemDetail.Detail;
            _logger.LogInformation($"Log Problems That Acured: {s}");
            await maintenanceApp.UpdateMaintenanceTypeAsync(maintenanceType);
        }

        [HttpDelete("maintenancetype/{id}")]
        public async void DeleteMaintenanceTypeAsync(int id)
        {
            var s = problemDetail.Status;
            _logger.LogInformation($"Log Status code: {s}");
            await maintenanceApp.DeleteMaintenanceTypeAsync(id);
        }

        [HttpGet("planesdue")]
        public IEnumerable<PlanesDueForMaintenance> GetPlanesDue()
        {
            _logger.LogDebug("Getting planes due in API");
            //_logger.LogWarning("Warning: Getting planes due in the API");
            problemDetail.Detail = "Warning: Getting planes due in the API";
            return maintenanceApp.GetPlanesDue();
        }

        [HttpGet("mechanic/schedule")]
        public async Task<string> GetMaintenanceAsync()
        {
            var mechs = await maintenanceApp.GetMaintenanceAsync();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(mechs, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            return json;
        }

        [HttpPost("mechanics")]
        public async Task<string> GetAvailableMechanicsForTimeRange(DateTime start, DateTime end)
        {
            var mechs = await maintenanceApp.GetAvailableMechanicsForTimeRange(start, end);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(mechs, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            problemDetail.Detail = $"start date was: {start}, end date was:{end}";
            _logger.LogInformation(problemDetail.Detail);
            return json;
        }

        [HttpGet("airplanes")]
        public async Task<IEnumerable<Airplane>> GetAirplanes()
        {
            return await maintenanceApp.GetAirplanes();
        }

        [HttpGet("hangers")]
        public async Task<IEnumerable<Hanger>> GetHangers()
        {
            return await maintenanceApp.GetHangers();
        }

        [HttpPost("schedulenew")]
        public async Task<bool> PostNewMaintenance(NewMaintenance newMaintenance)
        {
            return await maintenanceApp.AddMaintenance(newMaintenance);
        }

        [HttpGet("person/{email}")]
        public async Task<Person> GetPersonByEmail(string email)
        {
            return await maintenanceApp.GetPersonByEmail(email);
        }

        [HttpPost("person/update")]
        public async Task<bool> UpdatePerson(Person person)
        {
            return await maintenanceApp.UpdatePerson(person);
        }

    }
}
