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
        IQueryable<AnimalDto> QueryAnimals();
        IQueryable<BullDto> QueryBulls();
        IQueryable<CalfDto> QueryCalves();
        IQueryable<DryCowDto> QueryDryCows();
        IQueryable<HeiferDto> QueryHeifers();
        IQueryable<MilkingCowDto> QueryMilkingCows();
        void UpdateAnimal(FarmAnimal animal);
    }
}