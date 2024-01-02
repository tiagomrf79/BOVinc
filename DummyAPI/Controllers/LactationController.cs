using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LactationController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<LactationController> _logger;
    private readonly IDistributedCache _distributedCache;


    public LactationController(ILogger<LactationController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }

    [HttpGet("Dropdown", Name = "GetLactationsForDropdown")]
    [SwaggerOperation(Summary = "Gets an animal lactation options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of lactation options", typeof(IEnumerable<OptionForDropdownDto>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns a standard error response", typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> Get(
        [FromQuery, SwaggerParameter("Animal ID", Required = true)] int animalId)
    {
        //if lactation was not found
        if (animalId == 0)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with ID {animalId} does not exist."
            };

            _logger.LogInformation("The animal with ID {id} does not exist.", animalId);

            return NotFound(problemDetails);
        }

        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "3" },
            new OptionForDropdownDto { Id = 2, Name = "4" },
            new OptionForDropdownDto { Id = 3, Name = "5" },
            new OptionForDropdownDto { Id = 4, Name = "6" },
        };

        return Ok(listToReturn);
    }
}
