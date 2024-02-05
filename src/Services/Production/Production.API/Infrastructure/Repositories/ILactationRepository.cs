using Production.API.Models;

namespace Production.API.Infrastructure.Repositories;

public interface ILactationRepository
{
    Task<Lactation?> GetLactationByIdAsync(int id);
    Task<Lactation?> GetLactationByDateAsync(int animalId, DateOnly date);
    Task<Lactation?> GetSubsequentLactationAsync(Lactation lactation);
}
