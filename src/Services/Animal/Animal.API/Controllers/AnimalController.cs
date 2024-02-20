using Animal.API.DTOs;
using Animal.API.Enums;
using Animal.API.Infrastructure.Repositories;
using Animal.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;

namespace Animal.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AnimalController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;
    private readonly ILactationRepository _lactationRepository;
    private readonly IAnimalStatusRepository _animalStatusRepository;
    private readonly ILogger<FarmAnimal> _logger;
    private readonly IMapper _mapper;

    public AnimalController(
        IAnimalRepository animalRepository,
        ILactationRepository lactationRepository,
        IAnimalStatusRepository animalStatusRepository,
        ILogger<FarmAnimal> logger,
        IMapper mapper)
    {
        _animalRepository = animalRepository;
        _lactationRepository = lactationRepository;
        _animalStatusRepository = animalStatusRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost]
    public Task<ActionResult> CreateAnimal()
    {
        throw new NotImplementedException("To create an animal we need context and possibly its recent history...");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAnimal([FromBody] AnimalUpdateDto dataReceived)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for updating animal {id} with data {DataReceived}",
            nameof(UpdateAnimal), dataReceived.Id, dataReceived);

        // Check if record exists
        var recordToUpdate = await _animalRepository.GetAnimalByIdAsync(dataReceived.Id);
        if (recordToUpdate == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with id {dataReceived.Id} does not exist."
            };

            return NotFound(problemDetails);
        }

        _mapper.Map(dataReceived, recordToUpdate, typeof(AnimalUpdateDto), typeof(FarmAnimal));

        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(
            instance: recordToUpdate,
            validationContext: new ValidationContext(recordToUpdate),
            validationResults: validationResults,
            validateAllProperties: false);

        // Check if data received is valid in the entity
        if (!isValid)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"The request did not pass some validations."
            };

            _logger.LogInformation("Request failed the following validations: {validationResults}", validationResults);

            return BadRequest(problemDetails);
        }

        if (dataReceived.RegistrationId != null)
        {
            bool alreadyExists = await _animalRepository.GetAnimalByRegistrationIdAsync(dataReceived.RegistrationId) != null;
            if (alreadyExists)
            {
                ProblemDetails problemDetails = new()
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                    Title = "Invalid request.",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = $"The animal {dataReceived.RegistrationId} already exists."
                };

                return BadRequest(problemDetails);
            }
        }

        _animalRepository.UpdateAnimal(recordToUpdate);
        await _animalRepository.CommitChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public Task<ActionResult> DeleteAnimal([FromQuery] int animalId)
    {
        throw new NotImplementedException("Deleting an animal will move him to another table or something...");
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnimalDetailDto>> GetAnimal(int id)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for getting details for animal {id}",
            nameof(GetAnimal), id);

        //check if animal exists
        FarmAnimal? searchResult = await _animalRepository.GetAnimalByIdAsync(id);
        if (searchResult == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with id {id} does not exist."
            };

            return NotFound(problemDetails);
        }

        int lastLactationNumber = 0;
        if (Enumeration.FromValue<Sex>(searchResult.SexId) == Sex.Female)
        {
            IEnumerable<Lactation> lactations = await _lactationRepository.GetLactationsAsync(id);
            lastLactationNumber = lactations.Last().LactationNumber;
        }

        var dtoToReturn = _mapper.Map<AnimalDetailDto>(searchResult, opt => opt.Items["LastLactationNumber"] = lastLactationNumber);

        return Ok(dtoToReturn);
    }

    [HttpGet]
    public async Task<ActionResult<TableResponseDto<AnimalDto>>> GetAnimals([FromQuery] AnimalQueryDto<AnimalDto> searchQuery)
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetAnimals));

        var results = await _animalRepository.QueryAnimals();
        var query = results.AsQueryable();
        TableResponseDto<AnimalDto> dtoToReturn = CreateResponseDto(query, searchQuery);

        _logger.LogInformation("Returning {count} animals.", dtoToReturn.TotalCount);

        return Ok(dtoToReturn);
    }

    [HttpGet("Calves")]
    public async Task<ActionResult<TableResponseDto<CalfDto>>> GetCalves([FromQuery] AnimalQueryDto<CalfDto> searchQuery)
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetHeifers));

        var results = await _animalRepository.QueryCalves();
        var query = results.AsQueryable();
        TableResponseDto<CalfDto> dtoToReturn = CreateResponseDto(query, searchQuery);

        _logger.LogInformation("Returning {count} calves.", dtoToReturn.TotalCount);

        return Ok(dtoToReturn);
    }

    [HttpGet("Heifers")]
    public async Task<ActionResult<TableResponseDto<HeiferDto>>> GetHeifers([FromQuery] AnimalQueryDto<HeiferDto> searchQuery)
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetHeifers));

        var results = await _animalRepository.QueryHeifers();
        var query = results.AsQueryable();
        TableResponseDto<HeiferDto> dtoToReturn = CreateResponseDto(query, searchQuery);

        _logger.LogInformation("Returning {count} heifers.", dtoToReturn.TotalCount);

        return Ok(dtoToReturn);
    }

    [HttpGet("MilkingCows")]
    public async Task<ActionResult<TableResponseDto<MilkingCowDto>>> GetMilkingCows([FromQuery] AnimalQueryDto<MilkingCowDto> searchQuery)
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetMilkingCows));

        var results = await _animalRepository.QueryMilkingCows();
        var query = results.AsQueryable();
        TableResponseDto<MilkingCowDto> dtoToReturn = CreateResponseDto(query, searchQuery);

        _logger.LogInformation("Returning {count} milking cows.", dtoToReturn.TotalCount);

        return Ok(dtoToReturn);
    }

    [HttpGet("DryCows")]
    public async Task<ActionResult<TableResponseDto<DryCowDto>>> GetDryCows([FromQuery] AnimalQueryDto<DryCowDto> searchQuery)
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetDryCows));

        var results = await _animalRepository.QueryDryCows();
        var query = results.AsQueryable();
        TableResponseDto<DryCowDto> dtoToReturn = CreateResponseDto(query, searchQuery);

        _logger.LogInformation("Returning {count} dry cows.", dtoToReturn.TotalCount);

        return Ok(dtoToReturn);
    }

    [HttpGet("Bulls")]
    public async Task<ActionResult<TableResponseDto<BullDto>>> GetBulls([FromQuery] AnimalQueryDto<BullDto> searchQuery)
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetBulls));

        var results = await _animalRepository.QueryBulls();
        var query = results.AsQueryable();
        TableResponseDto<BullDto> dtoToReturn = CreateResponseDto(query, searchQuery);

        _logger.LogInformation("Returning {count} bulls.", dtoToReturn.TotalCount);

        return Ok(dtoToReturn);
    }

    [HttpGet("descendants/{animalId:int}")]
    public async Task<ActionResult<IEnumerable<DescendantDto>>> GetDescendants(int animalId)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for getting descendants for animal {id}",
            nameof(GetDescendants), animalId);

        FarmAnimal? parent = await _animalRepository.GetAnimalByIdAsync(animalId);

        //exit if parent is not found
        if (parent == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with id {animalId} does not exist."
            };

            return NotFound(problemDetails);
        }

        var descendants = await _animalRepository.GetDescendantsSortedByAge(parent);

        IEnumerable<DescendantDto> listToReturn = descendants
            .Select(x => new DescendantDto(
                x.Id,
                CreateLabelForAnimal(x),
                x.SexId,
                x.IsActive,
                x.Dam != null ? CreateLabelForAnimal(x.Dam) : String.Empty,
                x.Sire != null ? CreateLabelForAnimal(x.Sire) : String.Empty
            ));

        return Ok(listToReturn);
    }

    [HttpGet("ascendants/{animalId:int}")]
    public async Task<ActionResult<AscendantDto>> GetAscendants(int animalId)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for getting ascendants for animal {id}",
            nameof(GetAscendants), animalId);

        FarmAnimal? offspring = await _animalRepository.GetAnimalByIdAsync(animalId);

        //exit if offspring is not found
        if (offspring == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with id {animalId} does not exist."
            };

            return NotFound(problemDetails);
        }

        return CreateAscendantFromAnimal(offspring, 0);
    }

    [HttpGet("Dams")]
    public async Task<ActionResult<IEnumerable<ParentDto>>> GetDamOptions(DateOnly dateOfBirth)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for getting dam options for date {date}",
            nameof(GetDamOptions), dateOfBirth);

        var query = _animalRepository.GetPossibleDams(dateOfBirth);

        var listToReturn = await query
            .Select(x =>
                new ParentDto(
                    x.Id,
                    CreateLabelForAnimal(x),
                    x.Catalog.Name))
            .ToListAsync();

        _logger.LogInformation("Returning {count} dams.", listToReturn.Count);

        return Ok(listToReturn);
    }

    [HttpGet("Sires")]
    public async Task<ActionResult<IEnumerable<ParentDto>>> GetSireOptions(DateOnly dateOfBirth)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for getting sire options for date {date}",
            nameof(GetSireOptions), dateOfBirth);

        var query = _animalRepository.GetPossibleSires(dateOfBirth);

        var listToReturn = await query
            .Select(x =>
                new ParentDto(
                    x.Id,
                    CreateLabelForAnimal(x),
                    x.Catalog.Name))
            .ToListAsync();

        _logger.LogInformation("Returning {count} sires.", listToReturn.Count);

        return Ok(listToReturn);
    }

    [HttpGet("status/{id:int}")]
    public async Task<ActionResult<AnimalStatusDto>> GetAnimalStatus(int id)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for getting status for animal {id}",
            nameof(GetAnimalStatus), id);

        //check if animal exists
        FarmAnimal? animal = await _animalRepository.GetAnimalByIdAsync(id);
        if (animal == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with id {id} does not exist."
            };

            return NotFound(problemDetails);
        }

        int lastLactationNumber = 0;
        if (Enumeration.FromValue<Sex>(animal.SexId) == Sex.Female)
        {
            IEnumerable<Lactation> lactations = await _lactationRepository.GetLactationsAsync(id);
            lastLactationNumber = lactations.Last().LactationNumber;
        }

        AnimalStatus? animalStatus = await _animalStatusRepository.GetAnimalStatusById(id);
        if (animalStatus == null)
            animalStatus = new AnimalStatus() { Animal = animal };

        var dtoToReturn = new AnimalStatusDto(
            animal.Id,
            animal.IsActive,
            animal.DateOfBirth,
            animalStatus.CurrentGroupName,
            animalStatus.DateLeftHerd,
            animalStatus.ReasonLeftHerd,
            lastLactationNumber,
            animalStatus.MilkingStatus != null ? animalStatus.MilkingStatus.Id : null,
            animalStatus.MilkingStatus != null ? animalStatus.MilkingStatus.Name : null,
            animalStatus.LastCalvingDate,
            animalStatus.SheduledDryDate,
            animalStatus.LastDryDate,
            animalStatus.BreedingStatus != null ? animalStatus.BreedingStatus.Id : null,
            animalStatus.BreedingStatus != null ? animalStatus.BreedingStatus.Name : null,
            animalStatus.LastHeatDate,
            animalStatus.ExpectedHeatDate,
            animalStatus.LastBreedingDate,
            animalStatus.LastBreedingBull,
            animalStatus.DueDateForCalving
        );

        return Ok(dtoToReturn);


    }

    private TableResponseDto<T> CreateResponseDto<T>(IQueryable<T> query, AnimalQueryDto<T> searchQuery)
    {
        query = FilterQuery(query, searchQuery);

        int totalCount = query.Count();

        var listToReturn = query
            .AsQueryable()
            .OrderBy($"{searchQuery.SortAttribute} {searchQuery.SortDirection}")
            .Skip(searchQuery.StartIndex)
            .Take(searchQuery.MaxRecords)
            .ToList();

        var dtoToReturn = new TableResponseDto<T>()
        {
            TotalCount = totalCount,
            Rows = listToReturn
        };

        return dtoToReturn;
    }
    
    private IQueryable<T> FilterQuery<T>(IQueryable<T> query, AnimalQueryDto<T> searchQuery)
    {
        // filter by search keyword
        query = FilterQueryByKeyword(query, searchQuery.SearchKeyword);

        // filter by breed
        query = FilterQueryByOption(query, "BreedId", searchQuery.BreedFilter);

        // filter by breeding status
        query = FilterQueryByOption(query, "BreedingStatusId", searchQuery.BreedingStatusFilter);

        // filter by sex
        query = FilterQueryByOption(query, "SexId", searchQuery.SexFilter);

        return query;
    }

    private IQueryable<T> FilterQueryByKeyword<T>(IQueryable<T> query, string? keyword)
    {
        if (!string.IsNullOrEmpty(keyword))
        {
            var nameProperty = typeof(T).GetProperty("Name");
            var registrationIdProperty = typeof(T).GetProperty("RegistrationId");

            if (nameProperty != null || registrationIdProperty != null)
            {
                var filter = "";

                if (nameProperty != null && registrationIdProperty != null)
                {
                    filter = string.Format("{0} != null && {0}.Contains(@0, StringComparison.OrdinalIgnoreCase)", nameProperty.Name);
                    filter = filter + " || ";
                    filter = filter + string.Format("{0} != null && {0}.Contains(@0, StringComparison.OrdinalIgnoreCase)", registrationIdProperty.Name);
                }
                else if (nameProperty != null)
                {
                    filter = string.Format("{0} != null && {0}.Contains(@0, StringComparison.OrdinalIgnoreCase)", nameProperty.Name);
                }
                else if (registrationIdProperty != null)
                {
                    filter = string.Format("{0} != null && {0}.Contains(@0, StringComparison.OrdinalIgnoreCase)", registrationIdProperty.Name);
                }

                query = query.Where(filter, keyword);
            }
        }

        return query;
    }

    private IQueryable<T> FilterQueryByOption<T>(IQueryable<T> query, string optionName, int optionValue)
    {
        if (optionValue > 0)
        {
            var property = typeof(T).GetProperty(optionName);

            if (property != null)
            {
                var filter = string.Format("{0} == @0", property.Name);
                query = query.Where(filter, optionValue);
            }
        }

        return query;
    }

    private string CreateLabelForAnimal(FarmAnimal animal)
    {
        string label = "";

        if (!string.IsNullOrEmpty(animal.Name))
            label += animal.Name;
        if (!string.IsNullOrEmpty(animal.Name) && !string.IsNullOrEmpty(animal.RegistrationId))
            label += " - ";
        if (!string.IsNullOrEmpty(animal.RegistrationId))
            label += animal.RegistrationId;

        return label;
    }

    private AscendantDto CreateAscendantFromAnimal(FarmAnimal animal, int level = 0)
    {
        string animalLabel = CreateLabelForAnimal(animal);
        int sexId = animal.SexId;
        IEnumerable<AscendantDto> parents = Enumerable.Empty<AscendantDto>();

        if (level <= 2)
        {
            if (animal.Dam != null)
                parents.Append(CreateAscendantFromAnimal(animal.Dam, level + 1));

            if (animal.Sire != null)
                parents.Append(CreateAscendantFromAnimal(animal.Sire, level + 1));
        }

        AscendantDto ascendantDto = new AscendantDto(animalLabel, sexId, parents);

        return ascendantDto;
    }
}
