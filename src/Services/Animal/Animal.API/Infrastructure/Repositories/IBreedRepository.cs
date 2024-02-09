using Animal.API.Models;

namespace Animal.API.Infrastructure.Repositories
{
    public interface IBreedRepository
    {
        Task CreateBreedAsync(Breed breed);
        void UpdateBreed(Breed breed);
        void DeleteBreed(Breed breed);
        Task CommitChangesAsync();
        Task<Breed?> GetBreedByIdAsync(int id);
        Task<Breed?> GetBreedByNameAsync(string breedName);
        Task<IEnumerable<Breed>> GetBreedsSortedByNameAsync();
    }
}