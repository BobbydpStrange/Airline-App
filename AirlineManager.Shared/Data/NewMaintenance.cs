namespace AirlineManager.Shared.Data;

public class NewMaintenance
{
    public int MaintenanceTypeId { get; set; }
    public int AirplaneId { get; set; }

    public int MechanicId { get; set; }
    public int HangerId { get; set; }
    public DateTime Assigned { get; set; }
}
