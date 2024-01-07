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


    [HttpGet("TableAll", Name = "GetAllAnimalsForTable")]
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
                Name = "Malhão",
                DateOfBirth = new DateOnly(2021,8,5),
                GenderId = 2,
                Gender = "Male",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                CategoryId = 5,
                Category = "Bulls"
            },
        };

        IQueryable<AnimalDto> query = existingAnimals.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery.SearchKeyword))
            query = query.Where(h => 
                h.Name != null && h.Name.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase)
                || h.RegistrationId != null && h.RegistrationId.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase));

        if (searchQuery.GenderFilter > 0)
            query = query.Where(a => a.GenderId == searchQuery.GenderFilter);

        if (searchQuery.BreedFilter > 0)
            query = query.Where(a => a.BreedId == searchQuery.BreedFilter);

        int totalCount = query.Count();

        query = query
            .OrderBy($"{searchQuery.SortAttribute} {searchQuery.SortDirection}")
            .Skip(searchQuery.StartIndex)
            .Take(searchQuery.MaxRecords);

        var listToReturn = query.ToList();

        var dtoToReturn = new AnimalForTableDto()
        {
            TotalCount = totalCount,
            Animals = listToReturn
        };

        return Ok(dtoToReturn);
    }

    [HttpGet("TableCalves", Name = "GetCalvesForTable")]
    [SwaggerOperation(Summary = "Retrieves a list of calves with custom paging, sorting, and filtering rules.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of calves and the total number of records", typeof(CalfForTableDto))]
    public async Task<ActionResult<CalfForTableDto>> GetCalves(
        [FromQuery, SwaggerParameter("Object with search, sort and pagination options", Required = true)] AnimalQueryDto<CalfDto> searchQuery)
    {
        var existingAnimals = new List<CalfDto>()
        {
            new CalfDto()
            {
                Id = 10,
                RegistrationId = "PT 295 977381",
                DateOfBirth = new DateOnly(2023,3,16),
                GenderId = 2,
                Gender = "Male",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                DamId = 1,
                DamName = "Marquesa",
                SireId = 13,
                IsCatalogSire = false,
                SireName = "Malhão"
            },
            new CalfDto()
            {
                Id = 11,
                RegistrationId = "PT 955 960691",
                DateOfBirth = new DateOnly(2023,5,14),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                DamId = null,
                DamName = "Pintada",
                SireId = 13,
                IsCatalogSire = true,
                SireName = "Tsubasa"
            },
        };

        IQueryable<CalfDto> query = existingAnimals.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery.SearchKeyword))
            query = query.Where(h =>
                h.RegistrationId != null && h.RegistrationId.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase));

        if (searchQuery.GenderFilter > 0)
            query = query.Where(a => a.GenderId == searchQuery.GenderFilter);

        if (searchQuery.BreedFilter > 0)
            query = query.Where(a => a.BreedId == searchQuery.BreedFilter);

        int totalCount = query.Count();

        query = query
            .OrderBy($"{searchQuery.SortAttribute} {searchQuery.SortDirection}")
            .Skip(searchQuery.StartIndex)
            .Take(searchQuery.MaxRecords);

        var listToReturn = query.ToList();

        var dtoToReturn = new CalfForTableDto()
        {
            TotalCount = totalCount,
            Calves = listToReturn
        };

        return Ok(dtoToReturn);
    }


    [HttpGet("TableHeifers", Name = "GetHeifersForTable")]
    [SwaggerOperation(Summary = "Retrieves a list of heifers with custom paging, sorting, and filtering rules.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of heifers and the total number of records", typeof(HeiferForTableDto))]
    public async Task<ActionResult<HeiferForTableDto>> GetHeifers(
        [FromQuery, SwaggerParameter("Object with search, sort and pagination options", Required = true)] AnimalQueryDto<HeiferDto> searchQuery)
    {
        var existingAnimals = new List<HeiferDto>()
        {
            new HeiferDto()
            {
                Id = 9,
                RegistrationId = "PT 394 975382",
                DateOfBirth = new DateOnly(2022,8,8),
                BreedingStatusId = 1,
                BreedingStatus = "Open",
                LastBreedingDate = null,
                DueDateForCalving = null
            },
            new HeiferDto()
            {
                Id = 12,
                RegistrationId = "PT 476 545412",
                DateOfBirth = new DateOnly(2021,12,31),
                BreedingStatusId = 3,
                BreedingStatus = "Confirmed",
                LastBreedingDate = new DateOnly(2023,6,19),
                DueDateForCalving = new DateOnly(2024,3,8)
            }
        };

        IQueryable<HeiferDto> query = existingAnimals.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery.SearchKeyword))
            query = query.Where(h =>
                h.RegistrationId != null && h.RegistrationId.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase));

        if (searchQuery.BreedingStatusFilter > 0)
            query = query.Where(a => a.BreedingStatusId == searchQuery.BreedingStatusFilter);

        int totalCount = query.Count();

        query = query
            .OrderBy($"{searchQuery.SortAttribute} {searchQuery.SortDirection}")
            .Skip(searchQuery.StartIndex)
            .Take(searchQuery.MaxRecords);

        var listToReturn = query.ToList();

        var dtoToReturn = new HeiferForTableDto()
        {
            TotalCount = totalCount,
            Heifers = listToReturn
        };

        return Ok(dtoToReturn);
    }


    [HttpGet("TableMilkingCows", Name = "GetMilkingCowsForTable")]
    [SwaggerOperation(Summary = "Retrieves a list of milking cows with custom paging, sorting, and filtering rules.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of milking cows and the total number of records", typeof(MilkingCowForTableDto))]
    public async Task<ActionResult<MilkingCowDto>> GetMilkingCows(
        [FromQuery, SwaggerParameter("Object with search, sort and pagination options", Required = true)] AnimalQueryDto<MilkingCowDto> searchQuery)
    {
        var existingAnimals = new List<MilkingCowDto>()
        {
            new MilkingCowDto()
            {
                Id = 1,
                RegistrationId = "PT 219 144848",
                Name = "Marquesa",
                LactationNumber = 5,
                LastCalvingDate = new DateOnly(2023,12,26),
                BreedingStatusId = 1,
                BreedingStatus = "Open",
                LastBreedingDate = null,
                DueDateForCalving = null
            },
            new MilkingCowDto()
            {
                Id = 3,
                RegistrationId = "PT 829 999162",
                Name = "Calçada",
                LactationNumber = 10,
                LastCalvingDate = new DateOnly(2023,3,17),
                BreedingStatusId = 3,
                BreedingStatus = "Confirmed",
                LastBreedingDate = new DateOnly(2023,5,31),
                DueDateForCalving = new DateOnly(2024,3,14)
            },
            new MilkingCowDto()
            {
                Id = 4,
                RegistrationId = "PT 337 386951",
                Name = "Tieta",
                LactationNumber = 4,
                LastCalvingDate = new DateOnly(2023,12,21),
                BreedingStatusId = 1,
                BreedingStatus = "Open",
                LastBreedingDate = null,
                DueDateForCalving = null
            },
            new MilkingCowDto()
            {
                Id = 5,
                RegistrationId = "PT 454 199443",
                Name = "Xepa",
                LactationNumber = 8,
                LastCalvingDate = new DateOnly(2023,12,26),
                BreedingStatusId = 2,
                BreedingStatus = "Bred",
                LastBreedingDate = new DateOnly(2023,12,17),
                DueDateForCalving = null
            },
            new MilkingCowDto()
            {
                Id = 6,
                RegistrationId = "PT 536 445260",
                Name = "Malhinha",
                LactationNumber = 1,
                LastCalvingDate = new DateOnly(2023,4,1),
                BreedingStatusId = 1,
                BreedingStatus = "Open",
                LastBreedingDate = null,
                DueDateForCalving = null
            },
            new MilkingCowDto()
            {
                Id = 7,
                RegistrationId = "PT 437 619249",
                Name = "Mimosa",
                LactationNumber = 4,
                LastCalvingDate = new DateOnly(2023,4,1),
                BreedingStatusId = 3,
                BreedingStatus = "Confirmed",
                LastBreedingDate = new DateOnly(2023,7,27),
                DueDateForCalving = new DateOnly(2024,5,9)
            }
        };

        IQueryable<MilkingCowDto> query = existingAnimals.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery.SearchKeyword))
            query = query.Where(h =>
                h.Name != null && h.Name.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase)
                || h.RegistrationId != null && h.RegistrationId.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase));

        if (searchQuery.BreedingStatusFilter > 0)
            query = query.Where(a => a.BreedingStatusId == searchQuery.BreedingStatusFilter);

        int totalCount = query.Count();

        query = query
            .OrderBy($"{searchQuery.SortAttribute} {searchQuery.SortDirection}")
            .Skip(searchQuery.StartIndex)
            .Take(searchQuery.MaxRecords);

        var listToReturn = query.ToList();

        var dtoToReturn = new MilkingCowForTableDto()
        {
            TotalCount = totalCount,
            MilkingCows = listToReturn
        };

        return Ok(dtoToReturn);
    }


    [HttpGet("TableDryCows", Name = "GetDryCowsForTable")]
    [SwaggerOperation(Summary = "Retrieves a list of dry cows with custom paging, sorting, and filtering rules.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of dry cows and the total number of records", typeof(DryCowTableDto))]
    public async Task<ActionResult<DryCowTableDto>> GetDryCows(
        [FromQuery, SwaggerParameter("Object with search, sort and pagination options", Required = true)] AnimalQueryDto<DryCowDto> searchQuery)
    {
        var existingAnimals = new List<DryCowDto>()
        {
            new DryCowDto()
            {
                Id = 2,
                RegistrationId = "PT 266 483932",
                Name = "",
                LactationNumber = 3,
                BreedingBullId = 13,
                IsCatalogSire = false,
                BreedingBullName = "Malhão",
                LastDryDate = new DateOnly(2023,11,11),
                DueDateForCalving = new DateOnly(2024,1,11)

            },
            new DryCowDto()
            {
                Id = 8,
                RegistrationId = "PT 404 653293",
                Name = "Francisca",
                LactationNumber = 3,
                BreedingBullId = null,
                IsCatalogSire = null,
                BreedingBullName = null,
                LastDryDate = new DateOnly(2023,11,11),
                DueDateForCalving = new DateOnly(2024,1,23)
            }
        };

        IQueryable<DryCowDto> query = existingAnimals.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery.SearchKeyword))
            query = query.Where(h =>
                h.Name != null && h.Name.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase)
                || h.RegistrationId != null && h.RegistrationId.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase));

        int totalCount = query.Count();

        query = query
            .OrderBy($"{searchQuery.SortAttribute} {searchQuery.SortDirection}")
            .Skip(searchQuery.StartIndex)
            .Take(searchQuery.MaxRecords);

        var listToReturn = query.ToList();

        var dtoToReturn = new DryCowTableDto()
        {
            TotalCount = totalCount,
            DryCows = listToReturn
        };

        return Ok(dtoToReturn);
    }


    [HttpGet("TableBulls", Name = "GetBullsForTable")]
    [SwaggerOperation(Summary = "Retrieves a list of bulls with custom paging, sorting, and filtering rules.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of bulls and the total number of records", typeof(BullForTableDto))]
    public async Task<ActionResult<BullForTableDto>> GetBulls(
        [FromQuery, SwaggerParameter("Object with search, sort and pagination options", Required = true)] AnimalQueryDto<BullDto> searchQuery)
    {
        var existingAnimals = new List<BullDto>()
        {
            new BullDto()
            {
                Id = 13,
                RegistrationId = "PT 119 264935",
                Name = "Malhão",
                DateOfBirth = new DateOnly(2021,8,5),
                BreedId = 1,
                Breed = "Holstein-Frisia",
            }
        };

        IQueryable<BullDto> query = existingAnimals.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery.SearchKeyword))
            query = query.Where(h =>
                h.Name != null && h.Name.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase)
                || h.RegistrationId != null && h.RegistrationId.Contains(searchQuery.SearchKeyword, StringComparison.OrdinalIgnoreCase));

        if (searchQuery.BreedFilter > 0)
            query = query.Where(a => a.BreedId == searchQuery.BreedFilter);

        int totalCount = query.Count();

        query = query
            .OrderBy($"{searchQuery.SortAttribute} {searchQuery.SortDirection}")
            .Skip(searchQuery.StartIndex)
            .Take(searchQuery.MaxRecords);

        var listToReturn = query.ToList();

        var dtoToReturn = new BullForTableDto()
        {
            TotalCount = totalCount,
            Bulls = listToReturn
        };

        return Ok(dtoToReturn);
    }


    [HttpGet("AnimalDetail", Name = "GetAnimalDetails")]
    [SwaggerOperation(Summary = "Gets the details (name, breed, etc) of a given animal")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the details of an animal", typeof(AnimalDetailDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns a standard error response", typeof(ProblemDetails))]
    public async Task<ActionResult<AnimalDetailDto>> GetById(
        [FromQuery, SwaggerParameter("Animal ID", Required = true)] int animalId)
    {
        List<AnimalDetailDto> existingAnimals = new()
        {
            new AnimalDetailDto()
            {
                Id = 1,
                IsActive = true,
                RegistrationId = "PT 219 144848",
                Name = "Marquesa",
                DateOfBirth = new DateOnly(2016, 8, 28),
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                DamId = 59,
                DamLabel = "Xepa",
                SireId = 75,
                SireLabel = "Cafuné - PT 266 874625",
                InventorySourceId = 1,
                InventorySource = "Initial inventory",
                PurposeId = 1,
                Purpose = "Milk",
                Notes ="Best cow ever",
                CategorySingular = "Milking Cow"
            },
            new AnimalDetailDto()
            {
                Id = 59,
                IsActive = false,
                Name = "Xepa",
                GenderId = 1,
                Gender = "Female",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                InventorySourceId = 2,
                InventorySource = "Historic record",
                LastLactationNumber = 8
            },
            new AnimalDetailDto()
            {
                Id = 13,
                IsActive = true,
                RegistrationId = "PT 119 264935",
                Name = "Malhão",
                DateOfBirth = new DateOnly(2021,8,5),
                GenderId = 2,
                Gender = "Male",
                BreedId = 1,
                Breed = "Holstein-Frisia",
                InventorySourceId = 3,
                InventorySource = "Calving",
                CategorySingular = "Bull"
            }

        };

        var dtoToReturn = existingAnimals.FirstOrDefault(a => a.Id == animalId);

        if (dtoToReturn is null)
            return BadRequest();

        return dtoToReturn;
    }


}
