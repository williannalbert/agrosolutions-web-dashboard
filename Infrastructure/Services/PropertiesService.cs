using AgroSolutions.Identity.Web.Application.DTOs;
using AgroSolutions.Identity.Web.Application.Interfaces;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AgroSolutions.Identity.Web.Infrastructure.Services;

public class PropertiesService : IPropertiesService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public PropertiesService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    private async Task SetAuthorizationHeader()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<FazendaDto>> GetFazendasByProdutorAsync(string produtorId)
    {
        await SetAuthorizationHeader();
        //var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<FazendaDto>>>($"/properties/v1/fazendas/produtor/{produtorId}");
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<FazendaDto>>>($"/properties/v1/fazendas");
        return response?.Data ?? new List<FazendaDto>();
    }

    public async Task<List<TalhaoDto>> GetTalhoesByFazendaAsync(Guid fazendaId)
    {
        await SetAuthorizationHeader();
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<TalhaoDto>>>($"/properties/v1/talhoes/fazenda/{fazendaId}");
        return response?.Data ?? new List<TalhaoDto>();
    }

    public async Task<List<SensorDto>> GetSensoresByTalhaoAsync(Guid talhaoId)
    {
        await SetAuthorizationHeader();
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<SensorDto>>>($"/properties/v1/sensores/talhao/{talhaoId}");
        return response?.Data ?? new List<SensorDto>();
    }
}