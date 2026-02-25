using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Headers;

namespace AgroSolutions.Identity.Web.Infrastructure.Auth;

public class UnauthorizedInterceptor : DelegatingHandler
{
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navManager;

    public UnauthorizedInterceptor(IJSRuntime jsRuntime, NavigationManager navManager)
    {
        _jsRuntime = jsRuntime;
        _navManager = navManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            _navManager.NavigateTo("/login", forceLoad: true);
        }

        return response;
    }
}
