using Animal.API.DTOs;
using Animal.API.Models;

namespace Animal.API.Infrastructure.Repositories
{
    public interface IAnimalRepository
    {
        Task CommitChangesAsync();
        Task<FarmAnimal?> GetAnimalByIdAsync(int id);
        Task<FarmAnimal?> GetAnimalByRegistrationIdAsync(string registrationId);
        Task<IEnumerable<FarmAnimal>> GetDescendantsSortedByAge(FarmAnimal parent);
        IQueryable<FarmAnimal> GetPossibleDams(DateOnly offspringDateOfBirth);
        IQueryable<FarmAnimal> GetPossibleSires(DateOnly offspringDateOfBirth);
        Task<IEnumerable<AnimalDto>> QueryAnimals();
        Task<IEnumerable<BullDto>> QueryBulls();
        Task<IEnumerable<CalfDto>> QueryCalves();
        Task<IEnumerable<DryCowDto>> QueryDryCows();
        Task<IEnumerable<HeiferDto>> QueryHeifers();
        Task<IEnumerable<MilkingCowDto>> QueryMilkingCows();
        void UpdateAnimal(FarmAnimal animal);
    }
}