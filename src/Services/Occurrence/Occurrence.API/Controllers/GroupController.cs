using Microsoft.AspNetCore.Mvc;
using Occurrence.API.Infrastructure.Repositories;
using Occurrence.API.Models;

namespace Occurrence.API.Controllers;

[Route("[controller]")]
[ApiController]
public class GroupController : ControllerBase
{
    private readonly IGroupRepository _groupRepository;
    private readonly ILogger _logger;

    public GroupController(IGroupRepository groupRepository, ILogger logger)
    {
        _groupRepository = groupRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetGroups));

        var listToReturn = await _groupRepository.GetGroupsSortedByIdAsync();

        return Ok(listToReturn);
    }
}
