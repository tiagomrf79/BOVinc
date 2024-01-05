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
                RegistrationId = "PT 219 144848",
                Name = "Marquesa",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 2,
                RegistrationId = "PT 266 483932",
                Name = "",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 3,
                RegistrationId = "PT 829 999162",
                Name = "Calçada",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 4,
                RegistrationId = "PT 337 386951",
                Name = "Tieta",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 5,
                RegistrationId = "PT 454 199443",
                Name = "Xepa",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 6,
                RegistrationId = "PT 536 445260",
                Name = "Malhinha",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 7,
                RegistrationId = "PT 437 619249",
                Name = "Mimosa",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 8,
                RegistrationId = "PT 404 653293",
                Name = "Francisca",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 59,
                RegistrationId = "",
                Name = "Xepa",
                Source = "Historic records"
            },
            new ParentOptionForDropdownDto
            {
                Id = 97,
                RegistrationId = "",
                Name = "Mimosa",
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
                RegistrationId = "PT 119 264935",
                Name = "Malhão",
                Source = "In farm"
            },
            new ParentOptionForDropdownDto
            {
                Id = 75,
                RegistrationId = "PT 266 874625",
                Name = "Cafuné",
                Source = "Historic records"
            },
            new ParentOptionForDropdownDto
            {
                Id = 459,
                RegistrationId = "CANM49887946546",
                Name = "Tsubasa",
                Source = "AI bull"
            },
            new ParentOptionForDropdownDto
            {
                Id = 497,
                RegistrationId = "CANM46798794664",
                Name = "Gecko",
                Source = "AI bull"
            },
        };

        return Ok(listToReturn);
    }
}
