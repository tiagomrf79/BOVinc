using Animal.API.Models;

namespace Animal.API.Infrastructure.Repositories
{
    public interface ILactationRepository
    {
        Task<IEnumerable<Lactation>> GetLactationsAsync(int animalId);
    }
}