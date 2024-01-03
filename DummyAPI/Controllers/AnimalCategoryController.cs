using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AnimalCategoryController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<AnimalCategoryController> _logger;
    private readonly IDistributedCache _distributedCache;

    public AnimalCategoryController(ILogger<AnimalCategoryController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }


    [HttpGet("Dropdown", Name = "GetCategoriesForDropdown")]
    [SwaggerOperation(Summary = "Gets category options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of category options", typeof(IEnumerable<OptionForDropdownDto>))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> Get()
    {
        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "Calves" },
            new OptionForDropdownDto { Id = 2, Name = "Heifers" },
            new OptionForDropdownDto { Id = 3, Name = "Milking Cows" },
            new OptionForDropdownDto { Id = 4, Name = "Dry Cows" },
            new OptionForDropdownDto { Id = 5, Name = "Bulls" },
        };

        return Ok(listToReturn);
    }

}
