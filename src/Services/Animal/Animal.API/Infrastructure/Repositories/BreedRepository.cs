using Animal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Animal.API.Infrastructure.Repositories;

public class BreedRepository : IBreedRepository
{
    private readonly AnimalContext _context;

    public BreedRepository(AnimalContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CreateBreedAsync(Breed breed)
    {
        breed.Name = breed.Name.Trim();
        breed.CreatedAt = DateTime.UtcNow;
        breed.LastUpdatedAt = DateTime.UtcNow;
        await _context.Breeds.AddAsync(breed);
    }

    public void UpdateBreed(Breed breed)
    {
        breed.Name = breed.Name.Trim();
        breed.LastUpdatedAt = DateTime.UtcNow;
        _context.Breeds.Update(breed);
    }

    public void DeleteBreed(Breed breed)
    {
        _context.Breeds.Remove(breed);
    }

    public async Task CommitChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Breed?> GetBreedByIdAsync(int id)
    {
        return await _context.Breeds.FindAsync(id);
    }

    public async Task<Breed?> GetBreedByNameAsync(string breedName)
    {
        breedName = breedName.Trim().ToLower();
        var breed = await _context.Breeds.Where(x => x.Name.ToLower() == breedName).FirstOrDefaultAsync();

        return breed;
    }

    public async Task<IEnumerable<Breed>> GetBreedsSortedByNameAsync()
    {
        var list = new List<Breed>();

        list = await _context.Breeds
            .OrderBy(x => x.Name)
            .ToListAsync();

        return list;
    }
}
