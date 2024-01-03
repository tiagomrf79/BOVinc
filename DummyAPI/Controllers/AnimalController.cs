using DummyAPI.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AnimalController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<AnimalController> _logger;
    private readonly IDistributedCache _distributedCache;


    public AnimalController(ILogger<AnimalController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }


    [HttpGet("Table", Name = "GetAllAnimalsForTable")]
    //todo: check if I need AnyOrigin
    [SwaggerOperation(Summary = "Retrieves a list of animals with custom paging, sorting, and filtering rules.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of animals", typeof(IEnumerable<AnimalForTableDto>))]
    public async Task<ActionResult<IEnumerable<AnimalForTableDto>>> GetAll(
        [FromQuery, SwaggerParameter("Object with search and pagination options", Required = true)] SearchQueryDto<AnimalForTableDto> searchQuery)
    {
        var listToReturn = new List<AnimalForTableDto>()
        {
            new AnimalForTableDto()
            {
                Id = 1,
                RegistrationId = "PT 219 144848",
                Name = "Marquesa",
                DateOfBirth = new DateOnly(2016,8,28),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 3,
                Category = "Milking Cows"
            },
            new AnimalForTableDto()
            {
                Id = 2,
                RegistrationId = "PT 266 483932",
                Name = "",
                DateOfBirth = new DateOnly(2012,7,29),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 2,
                Category = "Dry Cows"
            },
            new AnimalForTableDto()
            {
                Id = 3,
                RegistrationId = "PT 829 999162",
                Name = "Calçada",
                DateOfBirth = new DateOnly(2006,7,27),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 3,
                Category = "Milking Cows"
            },
            new AnimalForTableDto()
            {
                Id = 4,
                RegistrationId = "PT 337 386951",
                Name = "Tieta",
                DateOfBirth = new DateOnly(2011,12,1),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 3,
                Category = "Milking Cows"
            },
            new AnimalForTableDto()
            {
                Id = 5,
                RegistrationId = "PT 454 199443",
                Name = "Xepa",
                DateOfBirth = new DateOnly(1997,4,11),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 3,
                Category = "Milking Cows"
            },
            new AnimalForTableDto()
            {
                Id = 6,
                RegistrationId = "PT 536 445260",
                Name = "Malhinha",
                DateOfBirth = new DateOnly(1994,1,27),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 3,
                Category = "Milking Cows"
            },
            new AnimalForTableDto()
            {
                Id = 7,
                RegistrationId = "PT 437 619249",
                Name = "Mimosa",
                DateOfBirth = new DateOnly(2016,7,1),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 3,
                Category = "Milking Cows"
            },
            new AnimalForTableDto()
            {
                Id = 8,
                RegistrationId = "PT 404 653293",
                Name = "Francisca",
                DateOfBirth = new DateOnly(2007,4,27),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 4,
                Category = "Dry Cows"
            },
            new AnimalForTableDto()
            {
                Id = 9,
                RegistrationId = "PT 394 975382",
                Name = "",
                DateOfBirth = new DateOnly(2022,8,8),
                GenderId = 1,
                Gender = "Female",
                BreedId = 2,
                Breed = "Jersey",
                CategoryId = 2,
                Category = "Heifers"
            },
            new AnimalForTableDto()
            {
                Id = 10,
                RegistrationId = "PT 295 977381",
                Name = "",
                DateOfBirth = new DateOnly(2023,3,16),
                GenderId = 2,
                Gender = "Male",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 1,
                Category = "Calves"
            },
            new AnimalForTableDto()
            {
                Id = 11,
                RegistrationId = "PT 955 960691",
                Name = "",
                DateOfBirth = new DateOnly(2023,5,14),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 1,
                Category = "Calves"
            },
            new AnimalForTableDto()
            {
                Id = 12,
                RegistrationId = "PT 476 545412",
                Name = "",
                DateOfBirth = new DateOnly(2021,12,31),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 2,
                Category = "Heifers"
            },
            new AnimalForTableDto()
            {
                Id = 13,
                RegistrationId = "PT 119 264935",
                Name = "",
                DateOfBirth = new DateOnly(2021,8,5),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 2,
                Category = "Heifers"
            },
        };

        return Ok(listToReturn);
    }
}
