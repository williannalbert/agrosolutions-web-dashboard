using AgroSolutions.Identity.Web.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace AgroSolutions.Identity.Web.Infrastructure.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var session = AuthServiceMock.CurrentSession;

        var identity = new ClaimsIdentity(); 

        if (session != null)
        {
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, session.User.Name),
                new Claim(ClaimTypes.Email, session.User.Email),
                new Claim("access_token", session.AccessToken) 
            }, "CustomAuth"); 
        }

        var user = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(user));
    }

    public void NotifyAuthChange()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}