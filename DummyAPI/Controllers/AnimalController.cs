using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;

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
    [SwaggerOperation(Summary = "Retrieves a list of animals with custom paging, sorting, and filtering rules.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of animals and the total number of records", typeof(AnimalForTableDto))]
    public async Task<ActionResult<AnimalForTableDto>> GetAll(
        [FromQuery, SwaggerParameter("Object with search, sort and pagination options", Required = true)] AnimalQueryDto<AnimalDto> searchQuery)
    {
        var existingAnimals = new List<AnimalDto>()
        {
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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
            new AnimalDto()
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

        IQueryable<AnimalDto> query = existingAnimals.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery.SearchKeyword))
            query = query.Where(h => 
                h.Name.IndexOf(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase) != -1
                || h.RegistrationId.IndexOf(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase) != -1);

        if (searchQuery.GenderFilter > 0)
            query = query.Where(a => a.GenderId == searchQuery.GenderFilter);

        if (searchQuery.BreedFilter > 0)
            query = query.Where(a => a.BreedId == searchQuery.BreedFilter);

        int totalCount = query.Count();

        query = query
            .OrderBy($"{searchQuery.SortAttribute} {searchQuery.SortOrder}")
            .Skip(searchQuery.StartIndex * searchQuery.MaxRecords)
            .Take(searchQuery.MaxRecords);

        var listToReturn = query.ToList();

        var dtoToReturn = new AnimalForTableDto()
        {
            TotalCount = totalCount,
            Animals = listToReturn
        };

        return Ok(dtoToReturn);
    }
}
