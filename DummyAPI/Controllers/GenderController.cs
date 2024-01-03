using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class GenderController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<GenderController> _logger;
    private readonly IDistributedCache _distributedCache;

    public GenderController(ILogger<GenderController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }


    [HttpGet("Dropdown", Name = "GetGendersForDropdown")]
    [SwaggerOperation(Summary = "Gets gender options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of gender options", typeof(IEnumerable<OptionForDropdownDto>))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> Get()
    {
        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "Female" },
            new OptionForDropdownDto { Id = 2, Name = "Male" }
        };

        return Ok(listToReturn);
    }

}
