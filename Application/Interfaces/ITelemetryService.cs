using AgroSolutions.Identity.Web.Application.DTOs;
using AgroSolutions.Identity.Web.Domain.Models;

namespace AgroSolutions.Identity.Web.Application.Interfaces;

public interface ITelemetryService
{
    Task<List<Telemetry>> SearchAsync(TelemetryFilter filter);
}
