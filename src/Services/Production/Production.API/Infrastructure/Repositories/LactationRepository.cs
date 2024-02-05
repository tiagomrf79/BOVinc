using Microsoft.EntityFrameworkCore;
using Production.API.Models;

namespace Production.API.Infrastructure.Repositories;

public class LactationRepository : ILactationRepository
{
    private readonly ProductionContext _context;

    public LactationRepository(ProductionContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Lactation?> GetLactationByIdAsync(int id)
    {
        return await _context.Lactations.FindAsync(id);
    }

    public Task<Lactation?> GetLactationByDateAsync(int animalId, DateOnly date)
    {
        return _context.Lactations
            .Where(x =>
                x.AnimalId == animalId
                && x.CalvingDate <= date)
            .OrderByDescending(x => x.CalvingDate)
            .FirstOrDefaultAsync();
    }

    public async Task<Lactation?> GetSubsequentLactationAsync(Lactation lactation)
    {
        return await _context.Lactations
            .Where(x =>
                x.AnimalId == lactation.AnimalId
                && x.CalvingDate > lactation.CalvingDate)
            .OrderBy(x => x.CalvingDate)
            .FirstOrDefaultAsync();
    }
}
