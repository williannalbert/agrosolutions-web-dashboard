using AgroSolutions.Identity.Web.Application.DTOs;

namespace AgroSolutions.Identity.Web.Application.Interfaces;

public interface IAuthService
{
    Task<TokenResponse?> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}
