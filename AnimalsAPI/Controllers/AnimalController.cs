using AnimalsAPI.DbContexts;
using AnimalsAPI.DTOs;
using AnimalsAPI.Extensions;
using AnimalsAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace AnimalsAPI.Controllers;

[Route("api/[controller]")]
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

        string cacheKey = $"{nameof(Get)}-{id}";

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
}
