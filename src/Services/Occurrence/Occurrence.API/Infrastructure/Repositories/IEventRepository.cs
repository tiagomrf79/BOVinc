using Occurrence.API.Models;

namespace Occurrence.API.Infrastructure.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetEventsSortedByDateAsync(int animalId);
    }
}