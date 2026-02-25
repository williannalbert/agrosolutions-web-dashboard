using AgroSolutions.Identity.Web.Application.DTOs;

namespace AgroSolutions.Identity.Web.Application.Interfaces;

public interface IPropertiesService
{
    Task<List<FazendaDto>> GetFazendasByProdutorAsync(string produtorId);
    Task<List<TalhaoDto>> GetTalhoesByFazendaAsync(Guid fazendaId);
    Task<List<SensorDto>> GetSensoresByTalhaoAsync(Guid talhaoId);
}
