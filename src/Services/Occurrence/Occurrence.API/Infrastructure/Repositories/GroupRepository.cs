using Microsoft.EntityFrameworkCore;
using Occurrence.API.Models;

namespace Occurrence.API.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{
    private readonly OccurrenceContext _context;

    public GroupRepository(OccurrenceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Group>> GetGroupsSortedByIdAsync()
    {
        return await _context.Groups
            .OrderBy(x => x.Id)
            .ToListAsync();
    }
}
