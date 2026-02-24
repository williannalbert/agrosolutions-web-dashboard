using AgroSolutions.Identity.Web.Application.DTOs;
using AgroSolutions.Identity.Web.Application.Interfaces;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace AgroSolutions.Identity.Web.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<TokenResponse?> LoginAsync(LoginRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/identity/v1/login", request);

            if (response.IsSuccessStatusCode)
            {
                var apiResult = await response.Content.ReadFromJsonAsync<ApiResponse<TokenResponse>>();

                var tokenData = apiResult?.Data;

                if (tokenData != null && !string.IsNullOrEmpty(tokenData.AccessToken))
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", tokenData.AccessToken);
                    return tokenData;
                }
            }
            return null;
        }
        catch (Exception ex)
        {

            throw;
        }
        
    }

    public async Task LogoutAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
    }
}
