using Occurrence.API.Models;

namespace Occurrence.API.Infrastructure.Repositories
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetGroupsSortedByIdAsync();
    }
}