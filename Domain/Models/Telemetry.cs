namespace AgroSolutions.Identity.Web.Domain.Models;

public class Telemetry
{
    public string Id { get; set; } = string.Empty;
    public string SensorId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public Guid FieldId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string? Email { get; set; }

    public double? SoilMoisturePercent { get; set; }
    public double? SoilPh { get; set; }
    public double? Nitrogen { get; set; }
    public double? Phosphorus { get; set; }
    public double? Potassium { get; set; }

    public double? TempCelsius { get; set; }
    public double? HumidityPercent { get; set; }
    public double? WindSpeedKmh { get; set; }
    public string? WindDirection { get; set; }
    public double? RainMmLastHour { get; set; }
    public double? DewPoint { get; set; }

    public double? FillLevelPercent { get; set; }
    public double? AvgTempCelsius { get; set; }
    public double? Co2Ppm { get; set; }

    public string Summary => Type.ToLower() switch
    {
        "solo" => $"Umidade: {SoilMoisturePercent:F1}% | pH: {SoilPh:F1}",
        "meteorologica" => $"Temp: {TempCelsius:F1}°C | Chuva: {RainMmLastHour:F1}mm",
        "silo" => $"Nível: {FillLevelPercent:F1}% | CO2: {Co2Ppm:F0}ppm",
        _ => "Sem dados"
    };
}
