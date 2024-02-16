using Animal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Animal.API.Infrastructure.Repositories;

public class LactationRepository : ILactationRepository
{
    private readonly AnimalContext _context;

    public LactationRepository(AnimalContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Lactation>> GetLactationsAsync(int animalId)
    {
        var list = new List<Lactation>();

        list = await _context.Lactations
            .Where(x => x.FarmAnimal != null && x.FarmAnimal.Id == animalId)
            .OrderBy(x => x.LactationNumber)
            .ToListAsync();

        return list;
    }
}
