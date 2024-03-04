using Animal.API.DTOs;
using Animal.API.Enums;
using Animal.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Net.WebSockets;

namespace Animal.API.Infrastructure.Repositories;

public class AnimalRepository : IAnimalRepository
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

        if (parent.SexId == Sex.Male.Id)
            query = query.Where(x => x.Sire == parent);
        else if (parent.SexId == Sex.Female.Id)
            query = query.Where(x => x.Dam == parent);

        list = await query.OrderByDescending(x => x.DateOfBirth).ToListAsync();

        return list;
    }

    public async Task<IEnumerable<AnimalDto>> QueryAnimals()
    {
        return await _context.FarmAnimals
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
            ))
            .ToListAsync();
    }

    public async Task<IEnumerable<CalfDto>> QueryCalves()
    {
        return await _context.FarmAnimals
            .Where(x => x.Category == Category.Calf)
            .Select(x => new CalfDto(
                x.Id,
                x.RegistrationId,
                x.DateOfBirth.GetValueOrDefault(),
                x.SexId,
                x.Sex.Name,
                x.Breed.Id,
                x.Breed.Name,
                x.Dam != null ? x.Dam.Id : null,
                x.Dam != null ? x.Dam.Name : "",
                x.Sire != null ? x.Sire.Id : null,
                x.Sire != null ? x.Sire.Name : ""
            )).ToListAsync();
    }

    public async Task<IEnumerable<HeiferDto>> QueryHeifers()
    {
        return await (
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
        ).ToListAsync();
    }

    public async Task<IEnumerable<MilkingCowDto>> QueryMilkingCows()
    {
        var lastLactationsPerAnimal = (
            from element in _context.Lactations
            group element by element.FarmAnimal
                into groups
            select groups.OrderByDescending(g => g.CalvingDate).First()
        );

        var milkingCows = (
            from animal in _context.FarmAnimals
            where animal.CategoryId == Category.MilkingCow.Id
            join status in _context.AnimalStatus on animal.Id equals status.AnimalId into gj
            from subgroup in gj.DefaultIfEmpty()
            select new
            {
                animal.Id,
                animal.RegistrationId,
                animal.Name,
                subgroup.LastCalvingDate,
                subgroup.BreedingStatusId,
                BreedingStatusName = subgroup.BreedingStatus!.Name,
                subgroup.LastBreedingDate,
                subgroup.DueDateForCalving
            }).ToList();

        var result = (
            from a in milkingCows
            join b in lastLactationsPerAnimal on a.Id equals b.FarmAnimalId
            select new MilkingCowDto
            (
                a.Id,
                a.RegistrationId,
                a.Name,
                b.LactationNumber,
                (DateOnly)a.LastCalvingDate!,
                (int)a.BreedingStatusId!,
                a.BreedingStatusName,
                a.LastBreedingDate,
                a.DueDateForCalving
            )).ToList();

        return result;
    }

    public async Task<IEnumerable<DryCowDto>> QueryDryCows()
    {
        var lastLactationsPerAnimal = (
            from element in _context.Lactations
            group element by element.FarmAnimalId
                into groups
            select groups.OrderByDescending(g => g.CalvingDate).First()
        ).ToList();

        var dryCows = (
            from animal in _context.FarmAnimals
            where animal.CategoryId == Category.DryCow.Id
            join status in _context.AnimalStatus on animal.Id equals status.AnimalId into gj
            from subgroup in gj.DefaultIfEmpty()
            select new
            {
                animal.Id,
                animal.RegistrationId,
                animal.Name,
                subgroup.LastBreedingBull,
                subgroup.LastDryDate,
                subgroup.DueDateForCalving
            }).ToList();

        var result = (
            from a in dryCows
            join b in lastLactationsPerAnimal on a.Id equals b.FarmAnimalId
            select new DryCowDto
            (
                a.Id,
                a.RegistrationId,
                a.Name,
                b.LactationNumber,
                a.LastBreedingBull,
                a.LastDryDate.GetValueOrDefault(),
                a.DueDateForCalving.GetValueOrDefault()
            )).ToList();
        
        return result;
    }

    public async Task<IEnumerable<BullDto>> QueryBulls()
    {
        return await _context.FarmAnimals
            .Where(x => x.Category == Category.Bull)
            .Select(x => new BullDto(
                x.Id,
                x.RegistrationId,
                x.Name,
                x.DateOfBirth.GetValueOrDefault(),
                x.Breed.Id,
                x.Breed.Name
            )).ToListAsync();
    }

    public async Task<FarmAnimal?> GetAnimalByIdWithParents(int id)
    {
        return await _context.FarmAnimals
            .Where(x => x.Id == id)
            .Include(x => x.Dam)
            .Include(x => x.Dam != null ? x.Dam.Dam : null)
            .Include(x => x.Dam != null ? x.Dam.Sire : null)
            .Include(x => x.Sire)
            .Include(x => x.Sire != null ? x.Sire.Dam : null)
            .Include(x => x.Sire != null ? x.Sire.Sire : null)
            .FirstOrDefaultAsync();
    }

    public IQueryable<FarmAnimal> GetPossibleDams(DateOnly offspringDateOfBirth)
    {
        return _context.FarmAnimals
            .Where(x =>
                x.Sex == Sex.Female
                && x.DateOfBirth < offspringDateOfBirth.AddMonths(-18)
                && x.DateOfBirth > offspringDateOfBirth.AddYears(-20));
    }

    public IQueryable<FarmAnimal> GetPossibleSires(DateOnly offspringDateOfBirth)
    {
        return _context.FarmAnimals
            .Where(x =>
                x.Sex == Sex.Male
                && x.DateOfBirth < offspringDateOfBirth.AddMonths(-12)
                && x.DateOfBirth > offspringDateOfBirth.AddYears(-20));
    }
}
