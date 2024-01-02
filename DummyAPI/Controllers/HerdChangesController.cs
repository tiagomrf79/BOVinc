using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class HerdChangesController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<HerdChangesController> _logger;
    private readonly IDistributedCache _distributedCache;

    public HerdChangesController(ILogger<HerdChangesController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }


    [HttpGet("EnteringDropdown", Name = "GetReasonsForEnterForDropdown")]
    [SwaggerOperation(Summary = "Gets reason for entering herd options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of reason for entering herd options", typeof(IEnumerable<OptionForDropdownDto>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns a standard error response", typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> GetEnterReasons()
    {
        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "Born in Farm" },
            new OptionForDropdownDto { Id = 2, Name = "Initial Inventory" },
            new OptionForDropdownDto { Id = 3, Name = "Bought" }
        };

        return Ok(listToReturn);
    }


    [HttpGet("LeavingDropdown", Name = "GetReasonsForLeaveForDropdown")]
    [SwaggerOperation(Summary = "Gets reason for leaving herd options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of reason for leaving herd options", typeof(IEnumerable<OptionForDropdownDto>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns a standard error response", typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> GetLeaveReasons()
    {
        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "Died" },
            new OptionForDropdownDto { Id = 2, Name = "Sold" },
            new OptionForDropdownDto { Id = 3, Name = "Culled" }
        };

        return Ok(listToReturn);
    }

}
