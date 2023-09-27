using AutoMapper;
using FarmsAPI.DbContexts;
using FarmsAPI.DTO;
using FarmsAPI.Models;
using FarmsAPI.Validations;
using FluentValidation;
using FluentValidation.Results;
using HerdsAPI.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace FarmsAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class HerdController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<Herd> _validator;
    private readonly IMapper _mapper;

    public HerdController(ApplicationDbContext context, IValidator<Herd> validator, IMapper mapper)
    {
        _context = context;
        _validator = validator;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HerdDto), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404)]
    public async Task<IActionResult> GetHerd(int id)
    {
        Herd? herdFound = await _context.Herds.FindAsync(id);

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

        query = query
            .OrderBy($"{searchOptions.SortColumn} {searchOptions.SortOrder}")
            .Skip(searchOptions.PageIndex * searchOptions.PageSize)
            .Take(searchOptions.PageSize);

        List<Herd> herdsList = await query.ToListAsync();
        List<HerdDto> listToReturn = _mapper.Map<List<HerdDto>>(herdsList);
        
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

        return NoContent();
    }
}
