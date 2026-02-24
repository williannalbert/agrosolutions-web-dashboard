using AgroSolutions.Identity.Web;
using AgroSolutions.Identity.Web.Application.Interfaces;
using AgroSolutions.Identity.Web.Infrastructure.Auth;
using AgroSolutions.Identity.Web.Infrastructure.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://api.agrosolutions.site") });

builder.Services.AddScoped<ITelemetryService, TelemetryApiService>();
builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<IPropertiesService, PropertiesServiceMock>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
