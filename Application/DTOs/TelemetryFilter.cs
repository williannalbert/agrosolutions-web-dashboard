namespace AgroSolutions.Identity.Web.Application.DTOs;

public class TelemetryFilter
{
    public string? SensorId { get; set; }
    public string? FieldId { get; set; }
    public string? SensorType { get; set; } 
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
