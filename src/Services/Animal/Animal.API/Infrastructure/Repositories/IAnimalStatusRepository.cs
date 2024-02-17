using Animal.API.Models;

namespace Animal.API.Infrastructure.Repositories
{
    public interface IAnimalStatusRepository
    {
        Task<AnimalStatus?> GetAnimalStatusById(int id);
    }
}