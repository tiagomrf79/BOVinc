using AnimalsAPI.DbContexts;
using AnimalsAPI.DTOs;
using AnimalsAPI.Extensions;
using AnimalsAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;

namespace AnimalsAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AnimalController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly AnimalsDbContext _context;
    private readonly ILogger<AnimalController> _logger;
    private readonly IDistributedCache _distributedCache;
    private readonly IMapper _mapper;

    public AnimalController(AnimalsDbContext context,
                            ILogger<AnimalController> logger,
                            IDistributedCache distributedCache,
                            IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _distributedCache = distributedCache;
        _mapper = mapper;
    }

    /// <param name="id">The ID of the animal to retrieve.</param>
    /// <response code="200">Returns the requested animal.</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a single animal.", Description = "Retrieves a single animal with the given ID.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnimalResponseDto>> Get(int id)
    {
        _logger.LogInformation("Attempting to get animal with ID {id}", id);

        string cacheKey = $"AnimalsAPI-{nameof(Get)}-{id}";

        if (_distributedCache.TryGetValue(cacheKey, out Animal? animalFound))
        {
            _logger.LogInformation("Animal with ID {id} found in cache.", id);
        }
        else
        {
            try
            {
                //To ensure that it is thread-safe, we proceed only once the thread enters the semaphore. (CODE-MAZE)
                await semaphore.WaitAsync();

                if (_distributedCache.TryGetValue(cacheKey, out animalFound))
                {
                    _logger.LogInformation("Animal with ID {id} found in cache.", id);
                }
                else
                {
                    animalFound = await _context.Animals.FindAsync(id);

                    _logger.LogInformation("Animal with ID {id} fetched from database.", id);

                    var cacheEntryOptions = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

                    await _distributedCache.SetAsync(cacheKey, animalFound, cacheEntryOptions);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        if (animalFound == null)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with ID {id} does not exist."
            };

            _logger.LogInformation("The animal with ID {id} does not exist.", id);
            return NotFound(problemDetails);
        }

        AnimalResponseDto dtoToReturn = _mapper.Map<AnimalResponseDto>(animalFound);

        _logger.LogInformation("Returning animal with ID {id}", id);
        return Ok(dtoToReturn);
    }

    /// <param name="searchOptions">A DTO object that can be used to customize the data-retrieval parameters.</param>
    /// <response code="200">Returns a list of animals.</response>
    [HttpGet(Name = "SearchAnimals")]
    [SwaggerOperation(Summary = "Get a list of animals.", Description = "Retrieves a list of animals with custom paging, sorting, and filtering rules.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AnimalResponseDto>>> Get([FromQuery] SearchQueryDto<AnimalResponseDto> searchOptions)
    {
        _logger.LogInformation("Attempting to retrieve list of animals from query: {@searchOptions}", searchOptions);

        IQueryable<Animal> query = _context.Animals.AsQueryable();

        if (!string.IsNullOrEmpty(searchOptions.FilterQuery))
            query = query.Where(a =>
                ((a.Name != null) && a.Name.Contains(searchOptions.FilterQuery))
                || ((a.Tag != null) && a.Tag.Equals(searchOptions.FilterQuery))
                || ((a.RegistrationId != null) && a.RegistrationId.Contains(searchOptions.FilterQuery))
            );

        query = query
            .OrderBy($"{searchOptions.SortColumn} {searchOptions.SortOrder}")
            .Skip(searchOptions.PageIndex * searchOptions.PageSize)
            .Take(searchOptions.PageSize);

        List<Animal> animalsList = await query.ToListAsync();
        List<AnimalResponseDto> listToReturn = _mapper.Map<List<AnimalResponseDto>>(animalsList);

        _logger.LogInformation("Returning {listToReturn.Count} animals after search query.", listToReturn.Count);
        return Ok(listToReturn);
    }

    /// <param name="dtoReceived">A DTO object containing the data to create a new animal.</param>
    /// <response code="201">Returns the newly created animal's ID.</response>
    [HttpPost(Name = "CreateAnimal")]
    [SwaggerOperation(Summary = "Creates a animal.", Description = "Inserts a new animal into the database.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/json")]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult<int>> Post(AnimalUpdateDto dtoReceived)
    {
        _logger.LogInformation("Received request to create a new animal with data {dtoReceived}", dtoReceived);

        Animal animalToCreate = _mapper.Map<Animal>(dtoReceived);
        animalToCreate.DateCreated = DateTime.Now;

        await _context.AddAsync(animalToCreate);
        await _context.SaveChangesAsync();

        AnimalResponseDto dtoToReturn = _mapper.Map<AnimalResponseDto>(animalToCreate);

        _logger.LogInformation("New animal created with ID {dtoToReturn.Id}.", dtoToReturn.Id);
        return CreatedAtAction(nameof(Get), new { id = dtoToReturn.Id }, dtoToReturn.Id);
    }

    /// <param name="id">The ID of the animal to update.</param>
    /// <param name="dtoReceived">A DTO object containing the properties to update in the selected animal.</param>
    [HttpPut("{id:int}", Name = "UpdateAnimal")]
    [SwaggerOperation(Summary = "Updates a animal.", Description = "Updates the animal's data.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult> Put(int id, AnimalUpdateDto dtoReceived)
    {
        _logger.LogInformation("Received request to update animal with ID {id} and data {dtoReceived}", id, dtoReceived);

        Animal? animalToUpdate = await _context.Animals.FindAsync(id);

        if (animalToUpdate == null)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with ID {id} does not exist."
            };

            _logger.LogInformation("The animal with ID {id} does not exist.", id);
            return NotFound(problemDetails);
        }

        _mapper.Map(dtoReceived, animalToUpdate, typeof(AnimalResponseDto), typeof(Animal));
        animalToUpdate.DateUpdated = DateTime.Now;

        _context.Update(animalToUpdate);
        await _context.SaveChangesAsync();
        await _distributedCache.RemoveAsync($"AnimalsAPI-{nameof(Get)}-{id}");

        _logger.LogInformation($"Animal with ID {id} updated successfully.");
        return Ok();
    }

    /// <param name="id">The ID of the animal to delete.</param>
    [HttpDelete(Name = "DeleteAnimal")]
    [SwaggerOperation(Summary = "Deletes a animal.", Description = "Deletes a animal from the database.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Received request to delete animal with ID {id}.", id);

        Animal? animalToDelete = await _context.Animals.FindAsync(id);

        if (animalToDelete == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with ID {id} does not exist."
            };

            _logger.LogInformation("The animal with ID {id} does not exist.", id);
            return NotFound(problemDetails);
        }

        _context.Animals.Remove(animalToDelete);
        await _context.SaveChangesAsync();
        await _distributedCache.RemoveAsync($"AnimalsAPI-{nameof(Get)}-{id}");

        _logger.LogInformation($"Animal with ID {id} deleted successfully.");
        return NoContent();
    }
}
