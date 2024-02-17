using Animal.API.Models;

namespace Animal.API.Infrastructure.Repositories;

public class AnimalStatusRepository
{
    private readonly AnimalContext _context;

    public AnimalStatusRepository(AnimalContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<AnimalStatus?> GetAnimalStatusById(int id)
    {
        return await _context.AnimalStatus.FindAsync(id);
    }
}
