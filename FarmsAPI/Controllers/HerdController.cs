using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HerdsAPI.DbContexts;
using HerdsAPI.DTO;
using HerdsAPI.Extensions;
using HerdsAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Dynamic.Core;
using System.Text.Json;

namespace HerdsAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class HerdController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ApplicationDbContext _context;
    private readonly ILogger<HerdController> _logger;
    private readonly IDistributedCache _distributedCache;
    private readonly IValidator<Herd> _validator;
    private readonly IMapper _mapper;

    public HerdController(ApplicationDbContext context,
                          ILogger<HerdController> logger,
                          IDistributedCache distributedCache,
                          IValidator<Herd> validator,
                          IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _distributedCache = distributedCache;
        _validator = validator;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HerdDto), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> GetHerd(int id)
    {
        var cacheKey = $"{nameof(GetHerd)}-{id}";

        if (_distributedCache.TryGetValue(cacheKey, out Herd? herdFound))
        {
            _logger.Log(LogLevel.Information, "Herd found in cache.");
        }
        else
        {
            try
            {
                await semaphore.WaitAsync();

                if (_distributedCache.TryGetValue(cacheKey, out herdFound))
                {
                    _logger.Log(LogLevel.Information, "Herd found in cache.");
                }
                else
                {
                    herdFound = await _context.Herds.FindAsync(id);

                    _logger.Log(LogLevel.Information, "Herd fetched from database.");

                    var cacheEntryOptions = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

                    await _distributedCache.SetAsync(cacheKey, herdFound, cacheEntryOptions);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        if (herdFound == null)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The herd with ID {id} does not exist."
            };

            return NotFound(problemDetails);
        }

        HerdDto dtoToReturn = _mapper.Map<HerdDto>(herdFound);

        return Ok(dtoToReturn);
    }

    [HttpGet(Name = "SearchHerds")]
    [ProducesResponseType(typeof(List<HerdDto>), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    public async Task<IActionResult> SearchHerds([FromQuery] SearchQueryDto<HerdDto> searchOptions)
    {
        IQueryable<Herd> query = _context.Herds.AsQueryable();

        if (!string.IsNullOrEmpty(searchOptions.FilterQuery))
            query = query.Where(h => h.Name.Contains(searchOptions.FilterQuery));

        var cacheKey = $"{nameof(SearchHerds)}-{JsonSerializer.Serialize(searchOptions)}";

        if (_distributedCache.TryGetValue(cacheKey, out IEnumerable<HerdDto>? listToReturn))
        {
            _logger.Log(LogLevel.Information, "Herd list found in cache.");
        }
        else
        {
            try
            {
                //To ensure that it is thread-safe, we proceed only once the thread enters the semaphore. (CODE-MAZE)
                await semaphore.WaitAsync();

                if (_distributedCache.TryGetValue(cacheKey, out listToReturn))
                {
                    _logger.Log(LogLevel.Information, "Herd list found in cache.");
                }
                else
                {
                    query = query
                        .OrderBy($"{searchOptions.SortColumn} {searchOptions.SortOrder}")
                        .Skip(searchOptions.PageIndex * searchOptions.PageSize)
                        .Take(searchOptions.PageSize);

                    List<Herd> herdsList = await query.ToListAsync();

                    listToReturn = _mapper.Map<List<HerdDto>>(herdsList);

                    _logger.Log(LogLevel.Information, "Herd list fetched from database.");

                    var cacheEntryOptions = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));

                    await _distributedCache.SetAsync(cacheKey, listToReturn, cacheEntryOptions);
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        return Ok(listToReturn);
    }

    [HttpPost(Name = "CreateHerd")]
    [ProducesResponseType(typeof(HerdDto), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> CreateHerd(CreateHerdDto dtoReceived)
    {
        Herd herdToCreate = _mapper.Map<Herd>(dtoReceived);
        herdToCreate.DateCreated = DateTime.Now;

        ValidationResult result = await _validator.ValidateAsync(herdToCreate);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);

            ValidationProblemDetails details = new ValidationProblemDetails(ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            };

            return BadRequest(details);
        }

        await _context.AddAsync(herdToCreate);
        await _context.SaveChangesAsync();

        HerdDto dtoToReturn = _mapper.Map<HerdDto>(herdToCreate);

        return CreatedAtAction(nameof(GetHerd), new { id = dtoToReturn.Id }, dtoToReturn);
    }

    [HttpPut(Name = "UpdateHerd")]
    [ProducesResponseType(typeof(HerdDto), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> UpdateHerd(HerdDto dtoReceived)
    {
        Herd? herdToUpdate = await _context.Herds.FindAsync(dtoReceived.Id);

        if (herdToUpdate == null)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The herd with ID {dtoReceived.Id} does not exist."
            };

            return NotFound(problemDetails);
        }

        _mapper.Map(dtoReceived, herdToUpdate, typeof(HerdDto), typeof(Herd));
        herdToUpdate.DateUpdated = DateTime.Now;

        ValidationResult result = await _validator.ValidateAsync(herdToUpdate);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);

            ValidationProblemDetails details = new ValidationProblemDetails(ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            };

            return BadRequest(details);
        }

        _context.Update(herdToUpdate);
        await _context.SaveChangesAsync();
        await _distributedCache.RemoveAsync($"{nameof(GetHerd)}-{dtoReceived.Id}");

        return Ok(dtoReceived);
    }

    [HttpDelete(Name = "DeleteHerd")]
    [ProducesResponseType(typeof(NoContent), 204)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> DeleteHerd(int id)
    {
        Herd? herdToDelete = await _context.Herds.FindAsync(id);

        if (herdToDelete == null)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The herd with ID {id} does not exist."
            };

            return NotFound(problemDetails);
        }

        _context.Herds.Remove(herdToDelete);
        await _context.SaveChangesAsync();
        await _distributedCache.RemoveAsync($"{nameof(GetHerd)}-{id}");

        return NoContent();
    }
}
