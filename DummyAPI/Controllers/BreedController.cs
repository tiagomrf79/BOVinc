using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class BreedController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<BreedController> _logger;
    private readonly IDistributedCache _distributedCache;

    public BreedController(ILogger<BreedController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }


    [HttpGet("Dropdown", Name = "GetBreedsForDropdown")]
    [SwaggerOperation(Summary = "Gets breed options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of breed options", typeof(IEnumerable<OptionForDropdownDto>))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> Get()
    {
        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "Holstein-Frisia" },
            new OptionForDropdownDto { Id = 2, Name = "Jersey" },
            new OptionForDropdownDto { Id = 3, Name = "Brown Swiss" }
        };

        return Ok(listToReturn);
    }
}
