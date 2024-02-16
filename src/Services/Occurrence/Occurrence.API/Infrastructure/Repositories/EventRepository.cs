using Microsoft.EntityFrameworkCore;
using Occurrence.API.Models;

namespace Occurrence.API.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly OccurrenceContext _context;

    public EventRepository(OccurrenceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetEventsSortedByDateAsync(int animalId)
    {
        return await _context.Events
            .Where(x => x.AnimalId == animalId)
            .OrderByDescending(x => x.Date)
            .ToListAsync();
    }
}
