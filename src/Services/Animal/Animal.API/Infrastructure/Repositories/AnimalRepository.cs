using Animal.API.DTOs;
using Animal.API.Enums;
using Animal.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Animal.API.Infrastructure.Repositories;

public class AnimalRepository
{
    private readonly AnimalContext _context;

    public AnimalRepository(AnimalContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void UpdateAnimal(FarmAnimal animal)
    {
        if (animal.RegistrationId != null)
            animal.RegistrationId = animal.RegistrationId.Trim();
        if (animal.Name != null)
            animal.Name = animal.Name.Trim();
        if (animal.Notes != null)
            animal.Notes = animal.Notes.Trim();
        animal.LastUpdatedAt = DateTime.UtcNow;

        _context.FarmAnimals.Update(animal);
    }

    public async Task CommitChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<FarmAnimal?> GetAnimalByIdAsync(int id)
    {
        return await _context.FarmAnimals.FindAsync(id);
    }

    public async Task<FarmAnimal?> GetAnimalByRegistrationIdAsync(string registrationId)
    {
        return await _context.FarmAnimals
            .Where(x => x.RegistrationId == registrationId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<FarmAnimal>> GetDescendantsSortedByAge(FarmAnimal parent)
    {
        IEnumerable<FarmAnimal> list = Enumerable.Empty<FarmAnimal>();
        IQueryable<FarmAnimal> query = _context.FarmAnimals;

        if (parent.Sex == Sex.Male)
            query = query.Where(x => x.Sire == parent);
        else if (parent.Sex == Sex.Female)
            query = query.Where (x => x.Dam == parent);

        list = await query.OrderByDescending(x => x.DateOfBirth).ToListAsync();

        return list;
    }

    public IQueryable<AnimalDto> QueryAnimals()
    {
        return _context.FarmAnimals
            .Where(x => x.Category != null)
            .Select(x => new AnimalDto(
                x.Id,
                x.RegistrationId,
                x.Name,
                x.DateOfBirth.GetValueOrDefault(),
                x.Sex.Id,
                x.Sex.Name,
                x.Breed.Id,
                x.Breed.Name,
                x.Category!.Id,
                x.Category!.Name
            ));
    }

    public IQueryable<CalfDto> QueryCalves()
    {
        return _context.FarmAnimals
            .Where(x => x.Category == Category.Calf)
            .Select(x => new CalfDto(
                x.Id,
                x.RegistrationId,
                x.DateOfBirth.GetValueOrDefault(),
                x.Sex.Id,
                x.Sex.Name,
                x.Breed.Id,
                x.Breed.Name,
                x.Dam != null ? x.Dam.Id : null,
                x.Dam != null ? x.Dam.Name : "",
                x.Sire != null ? x.Sire.Id : null,
                x.Sire != null ? x.Sire.Name : ""
            ));
    }

    public IQueryable<HeiferDto> QueryHeifers()
    {
        return (
            from a in _context.FarmAnimals
            where a.Category == Category.Heifer
            join b in _context.AnimalStatus on a equals b.Animal into gj
            from res in gj.DefaultIfEmpty()
            select new HeiferDto(
                a.Id,
                a.RegistrationId,
                a.DateOfBirth.GetValueOrDefault(),
                res.BreedingStatus != null ? res.BreedingStatus.Id : BreedingStatus.Open.Id,
                res.BreedingStatus != null ? res.BreedingStatus.Name : BreedingStatus.Open.Name,
                res.LastBreedingDate,
                res.DueDateForCalving
            )
        );
    }

    public IQueryable<MilkingCowDto> QueryMilkingCows()
    {
        var lastLactationsPerAnimal = (
            from element in _context.Lactations
            group element by element.FarmAnimal
                into groups
            select groups.OrderByDescending(g => g.CalvingDate).First()
        );

        return (
            from a in _context.FarmAnimals
            where a.Category == Category.MilkingCow
            join b in lastLactationsPerAnimal on a equals b.FarmAnimal
            join c in _context.AnimalStatus on a equals c.Animal into gjc
            from res in gjc.DefaultIfEmpty()
            select new MilkingCowDto(
                a.Id,
                a.RegistrationId,
                a.Name,
                b.LactationNumber,
                res.LastCalvingDate.GetValueOrDefault(),
                res.BreedingStatus != null ? res.BreedingStatus.Id : BreedingStatus.Open.Id,
                res.BreedingStatus != null ? res.BreedingStatus.Name : BreedingStatus.Open.Name,
                res.LastBreedingDate,
                res.DueDateForCalving
            )
        );
    }

    public IQueryable<DryCowDto> QueryDryCows()
    {
        var lastLactationsPerAnimal = (
            from element in _context.Lactations
            group element by element.FarmAnimal
                into groups
            select groups.OrderByDescending(g => g.CalvingDate).First()
        );

        return (
            from a in _context.FarmAnimals
            where a.Category == Category.DryCow
            join b in lastLactationsPerAnimal on a equals b.FarmAnimal
            join c in _context.AnimalStatus on a equals c.Animal into gjc
            from res in gjc.DefaultIfEmpty()
            select new DryCowDto(
                a.Id,
                a.RegistrationId,
                a.Name,
                b.LactationNumber,
                res.LastBreedingBull,
                res.LastDryDate.GetValueOrDefault(),
                res.DueDateForCalving.GetValueOrDefault()
            )
        );
    }

    public IQueryable<BullDto> QueryBulls()
    {
        return _context.FarmAnimals
            .Where(x => x.Category == Category.Bull)
            .Select(x => new BullDto(
                x.Id,
                x.RegistrationId,
                x.Name,
                x.DateOfBirth.GetValueOrDefault(),
                x.Breed.Id,
                x.Breed.Name
            ));
    }

    public IQueryable<FarmAnimal> GetPossibleDams(DateOnly offspringDateOfBirth)
    {
        return _context.FarmAnimals
            .Where(x =>
                x.Sex == Sex.Female
                && x.DateOfBirth < offspringDateOfBirth.AddMonths(18)
                && x.DateOfBirth > offspringDateOfBirth.AddYears(20));
    }

    public IQueryable<FarmAnimal> GetPossibleSires(DateOnly offspringDateOfBirth)
    {
        return _context.FarmAnimals
            .Where(x =>
                x.Sex == Sex.Male
                && x.DateOfBirth < offspringDateOfBirth.AddMonths(12)
                && x.DateOfBirth > offspringDateOfBirth.AddYears(20));
    }
}
