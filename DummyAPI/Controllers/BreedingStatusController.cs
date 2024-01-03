using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BreedingStatusController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<BreedingStatusController> _logger;
    private readonly IDistributedCache _distributedCache;

    public BreedingStatusController(ILogger<BreedingStatusController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }


    [HttpGet("Dropdown", Name = "GetBreedingStatusForDropdown")]
    [SwaggerOperation(Summary = "Gets breeding status options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of breeding status options", typeof(IEnumerable<OptionForDropdownDto>))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> Get()
    {
        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "Open" },
            new OptionForDropdownDto { Id = 2, Name = "Bred" },
            new OptionForDropdownDto { Id = 3, Name = "Confirmed" },
        };

        return Ok(listToReturn);
    }
}
