using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class InventoryController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<AnimalController> _logger;
    private readonly IDistributedCache _distributedCache;

    public InventoryController(ILogger<AnimalController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }


    [HttpGet("DamDropdown", Name = "GetDamsForDropdown")]
    [SwaggerOperation(Summary = "Gets dam options for a dropdown, given a birth date")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of dam options", typeof(IEnumerable<ParentOptionForDropdownDto>))]
    public async Task<ActionResult<IEnumerable<ParentOptionForDropdownDto>>> GetDamOptions(
        [FromQuery, SwaggerParameter("Date of birth of the animal who's dam you're looking for", Required = false)] DateOnly birthDate)
    {
        //filter animals by sex
        //filter animals by age (age > birth date + 20M)

        var listToReturn = new List<ParentOptionForDropdownDto>()
        {
            new ParentOptionForDropdownDto
            {
                Id = 1,
                Label = "Marquesa - PT 219 144848",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 2,
                Label = "PT 266 483932",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 3,
                Label = "Calçada - PT 829 999162",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 4,
                Label = "Tieta - PT 337 386951",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 5,
                Label = "Xepa - PT 454 199443",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 6,
                Label = "Malhinha - PT 536 445260",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 7,
                Label = "Mimosa - PT 437 619249",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 8,
                Label = "Francisca - PT 404 653293",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 59,
                Label = "Xepa",
                Source = "Historic records"
            },
            new ParentOptionForDropdownDto
            {
                Id = 97,
                Label = "Mimosa",
                Source = "Historic records"
            },
        };

        return Ok(listToReturn);
    }


    [HttpGet("SireDropdown", Name = "GetSiresForDropdown")]
    [SwaggerOperation(Summary = "Gets sire options for a dropdown, given a birth date")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of sire options", typeof(IEnumerable<ParentOptionForDropdownDto>))]
    public async Task<ActionResult<IEnumerable<ParentOptionForDropdownDto>>> GetSireOptions(
        [FromQuery, SwaggerParameter("Date of birth of the animal who's sire you're looking for", Required = false)] DateOnly birthDate)
    {
        //filter animals by sex
        //filter animals by age, from birth date

        var listToReturn = new List<ParentOptionForDropdownDto>()
        {
            new ParentOptionForDropdownDto
            {
                Id = 13,
                Label = "Malhão - PT 119 264935",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 75,
                Label = "Cafuné - PT 266 874625",
                Source = "Historic records"
            },
            new ParentOptionForDropdownDto
            {
                Id = 459,
                Label = "Tsubasa - CANM49887946546",
                Source = "AI bull"
            },
            new ParentOptionForDropdownDto
            {
                Id = 497,
                Label = "Gecko - CANM46798794664",
                Source = "AI bull"
            },
        };

        return Ok(listToReturn);
    }


    [HttpGet("PurposeDropdown", Name = "GetPurposesForDropdown")]
    [SwaggerOperation(Summary = "Gets purpose options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of purpose options", typeof(IEnumerable<OptionForDropdownDto>))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> GetPurposeOptions()
    {
        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "Milk" },
            new OptionForDropdownDto { Id = 2, Name = "Meat" },
            new OptionForDropdownDto { Id = 3, Name = "To cull" },
            new OptionForDropdownDto { Id = 4, Name = "To sell" },
            new OptionForDropdownDto { Id = 4, Name = "Breeding" }
        };

        return Ok(listToReturn);
    }


    [HttpGet("InventorySourceDropdown", Name = "GetInventorySourceForDropdown")]
    [SwaggerOperation(Summary = "Gets inventory source options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of inventory source options", typeof(IEnumerable<OptionForDropdownDto>))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> GetInventorySourceOptions()
    {
        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "Initial inventory" },
            new OptionForDropdownDto { Id = 2, Name = "Historic record" },
            new OptionForDropdownDto { Id = 3, Name = "Calving" },
            new OptionForDropdownDto { Id = 4, Name = "From outside" }
        };

        return Ok(listToReturn);
    }

}
