using AutoMapper;
using FarmsAPI.DbContexts;
using FarmsAPI.DTO;
using FarmsAPI.Extensions;
using FarmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;

namespace FarmsAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class FarmController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ApplicationDbContext _context;
    private readonly ILogger<FarmController> _logger;
    private readonly IDistributedCache _distributedCache;
    private readonly IMapper _mapper;

    public FarmController(ApplicationDbContext context,
                          ILogger<FarmController> logger,
                          IDistributedCache distributedCache,
                          IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _distributedCache = distributedCache;
        _mapper = mapper;
    }

    /// <param name="id">The ID of the farm to retrieve.</param>
    /// <response code="200">Returns the requested farm.</response>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a single farm.", Description = "Retrieves a single farm with the given ID.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FarmResponseDto>> Get(int id)
    {
        _logger.LogInformation("Attempting to get farm with ID {id}.", id);

        string cacheKey = $"{nameof(Get)}-{id}";

        if (_distributedCache.TryGetValue(cacheKey, out Farm? farmFound))
        {
            _logger.LogInformation("Farm with ID {id} found in cache.", id);
        }
        else
        {
            try
            {
                //To ensure that it is thread-safe, we proceed only once the thread enters the semaphore. (CODE-MAZE)
                await semaphore.WaitAsync();

                if (_distributedCache.TryGetValue(cacheKey, out farmFound))
                {
                    _logger.LogInformation("Farm with ID {id} found in cache.", id);
                }
                else
                {
                    farmFound = await _context.Farms.FindAsync(id);

                    _logger.LogInformation("Farm with ID {id} fetched from database.", id);

                    var cacheEntryOptions = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

                    await _distributedCache.SetAsync(cacheKey, farmFound, cacheEntryOptions);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        if (farmFound == null)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The farm with ID {id} does not exist."
            };

            _logger.LogInformation("The farm with ID {id} does not exist.", id);
            return NotFound(problemDetails);
        }

        FarmResponseDto dtoToReturn = _mapper.Map<FarmResponseDto>(farmFound);

        _logger.LogInformation("Returning farm with ID {id}", id);
        return Ok(dtoToReturn);
    }

    /// <param name="searchOptions">A DTO object that can be used to customize the data-retrieval parameters.</param>
    /// <response code="200">Returns a list of farms.</response>
    [HttpGet(Name = "SearchFarms")]
    [SwaggerOperation(Summary = "Get a list of farms.", Description = "Retrieves a list of farms with custom paging, sorting, and filtering rules.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FarmResponseDto>>> Get([FromQuery] SearchQueryDto<FarmResponseDto> searchOptions)
    {
        _logger.LogInformation("Attempting to retrieve list of farms from query: {@searchOptions}", searchOptions);

        IQueryable<Farm> query = _context.Farms.AsQueryable();

        if (!string.IsNullOrEmpty(searchOptions.FilterQuery))
            query = query.Where(h => h.Name.Contains(searchOptions.FilterQuery));

        query = query
            .OrderBy($"{searchOptions.SortColumn} {searchOptions.SortOrder}")
            .Skip(searchOptions.PageIndex * searchOptions.PageSize)
            .Take(searchOptions.PageSize);

        List<Farm> farmsList = await query.ToListAsync();
        List<FarmResponseDto> listToReturn = _mapper.Map<List<FarmResponseDto>>(farmsList);

        _logger.LogInformation("Returning {listToReturn.Count} farms after search query.", listToReturn.Count);
        return Ok(listToReturn);
    }

    /// <param name="dtoReceived">A DTO object containing the data to create a new farm.</param>
    /// <response code="201">Returns the newly created farm's ID.</response>
    [HttpPost(Name = "CreateFarm")]
    [SwaggerOperation(Summary = "Creates a farm.", Description = "Inserts a new farm into the database.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("application/json")]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult<int>> Post(FarmUpdateDto dtoReceived)
    {
        _logger.LogInformation("Received request to create a new farm with data {@dtoReceived}", dtoReceived);

        Farm farmToCreate = _mapper.Map<Farm>(dtoReceived);
        farmToCreate.DateCreated = DateTime.Now;

        await _context.AddAsync(farmToCreate);
        await _context.SaveChangesAsync();

        FarmResponseDto dtoToReturn = _mapper.Map<FarmResponseDto>(farmToCreate);

        _logger.LogInformation("New farm created with ID {dtoToReturn.Id}.", dtoToReturn.Id);
        return CreatedAtAction(nameof(Get), new { id = dtoToReturn.Id }, dtoToReturn.Id);
    }

    /// <param name="id">The ID of the farm to update.</param>
    /// <param name="dtoReceived">A DTO object containing the properties to update in the selected farm.</param>
    [HttpPut("{id:int}", Name = "UpdateFarm")]
    [SwaggerOperation(Summary = "Updates a farm.", Description = "Updates the farm's data.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("application/json")]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult> Put(int id, FarmUpdateDto dtoReceived)
    {
        _logger.LogInformation("Received request to update farm with ID {id} and data {@dtoReceived}", id, dtoReceived);

        Farm? farmToUpdate = await _context.Farms.FindAsync(id);

        if (farmToUpdate == null)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The farm with ID {id} does not exist."
            };

            _logger.LogInformation("The farm with ID {id} does not exist.", id);
            return NotFound(problemDetails);
        }

        _mapper.Map(dtoReceived, farmToUpdate, typeof(FarmResponseDto), typeof(Farm));
        farmToUpdate.DateUpdated = DateTime.Now;

        _context.Update(farmToUpdate);
        await _context.SaveChangesAsync();

        await _distributedCache.RemoveAsync($"{nameof(Get)}-{id}");

        _logger.LogInformation($"Farm with ID {id} updated successfully.");
        return Ok();
    }

    /// <param name="id">The ID of the farm to delete.</param>
    [HttpDelete(Name = "DeleteFarm")]
    [SwaggerOperation(Summary = "Deletes a farm.", Description = "Deletes a farm from the database.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ResponseCache(NoStore = true)]
    public async Task<ActionResult> Delete(int id)
    {
        _logger.LogInformation("Received request to delete farm with ID {id}.", id);

        Farm? farmToDelete = await _context.Farms.FindAsync(id);

        if (farmToDelete == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The farm with ID {id} does not exist."
            };

            _logger.LogInformation("The farm with ID {id} does not exist.", id);
            return NotFound(problemDetails);
        }

        _context.Farms.Remove(farmToDelete);
        await _context.SaveChangesAsync();

        await _distributedCache.RemoveAsync($"{nameof(Get)}-{id}");

        _logger.LogInformation($"Farm with ID {id} deleted successfully.");
        return NoContent();
    }
}
