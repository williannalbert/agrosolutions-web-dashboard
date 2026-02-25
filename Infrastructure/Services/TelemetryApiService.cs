using AgroSolutions.Identity.Web.Application.DTOs;
using AgroSolutions.Identity.Web.Application.Interfaces;
using AgroSolutions.Identity.Web.Domain.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace AgroSolutions.Identity.Web.Infrastructure.Services;

public class TelemetryApiService : ITelemetryService
{
    private readonly HttpClient _http;

    public TelemetryApiService(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient("HistoryAPI");
    }
    public async Task<List<Telemetry>> SearchAsync(TelemetryFilter filter)
    {
        try
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter.SensorId))
                queryParams.Add($"sensor_id={filter.SensorId}");

            if (!string.IsNullOrWhiteSpace(filter.FieldId))
                queryParams.Add($"field_id={filter.FieldId}");

            if (!string.IsNullOrWhiteSpace(filter.SensorType))
                queryParams.Add($"type_sensor={filter.SensorType}");

            if (filter.StartDate.HasValue)
                queryParams.Add($"start_date={Uri.EscapeDataString(filter.StartDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"))}");
            if (filter.EndDate.HasValue)
                queryParams.Add($"end_date={Uri.EscapeDataString(filter.EndDate.Value.ToString("yyyy-MM-ddTHH:mm:ssZ"))}");

            if (!queryParams.Any()) return new List<Telemetry>();

            var url = $"api/History?{string.Join("&", queryParams)}";
            Console.WriteLine($"[TelemetryApiService] GET: {url}");

            var jsonArray = await _http.GetFromJsonAsync<JsonNode[]>(url);

            if (jsonArray == null || jsonArray.Length == 0) return new List<Telemetry>();

            return ParseJson(jsonArray);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO API]: {ex.Message}");
            return new List<Telemetry>();
        }
    }

    private List<Telemetry> ParseJson(JsonNode[] jsonArray)
    {
        var result = new List<Telemetry>();
        foreach (var item in jsonArray)
        {
            try
            {
                var t = new Telemetry();

                t.Id = GetString(item, "id");
                t.SensorId = GetString(item, "sensorId");
                t.Timestamp = item["timestamp"]?.GetValue<DateTime>() ?? DateTime.MinValue;
                t.FieldId = item["fieldId"]?.GetValue<Guid>() ?? Guid.Empty;
                t.Type = GetString(item, "sensorTypeDescription", "type", "sensorType").ToLower();
                t.Email = GetString(item, "email");

                var data = item["data"];
                if (data != null)
                {
                    t.SoilMoisturePercent = GetDouble(data, "soilMoisturePercent");
                    t.SoilPh = GetDouble(data, "soilPh");
                    t.Nitrogen = GetDouble(data["nutrients"], "nitrogen"); 

                    t.TempCelsius = GetDouble(data, "tempCelsius") ?? GetDouble(data, "temperatura"); 
                    t.RainMmLastHour = GetDouble(data, "rainMmLastHour");
                    t.HumidityPercent = GetDouble(data, "humidityPercent");

                    t.Co2Ppm = GetDouble(data, "co2Ppm");
                    t.FillLevelPercent = GetDouble(data, "fillLevelPercent");
                }
                result.Add(t);
            }
            catch { }
        }
        return result.OrderBy(x => x.Timestamp).ToList();
    }

    private string GetString(JsonNode? node, params string[] keys)
    {
        if (node == null) return "";
        foreach (var key in keys)
        {
            if (node[key] != null) return node[key]!.ToString();
            var pascal = char.ToUpper(key[0]) + key.Substring(1);
            if (node[pascal] != null) return node[pascal]!.ToString();
        }
        return "";
    }

    private double? GetDouble(JsonNode? node, string key)
    {
        if (node == null) return null;
        var val = node[key] ?? node[key.ToLower()];
        if (val == null) return null;
        try { return val.GetValue<double>(); } catch { return null; }
    }
}