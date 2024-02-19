using Animal.API.DTOs;
using Animal.API.Infrastructure.Repositories;
using Animal.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Animal.API.Controllers;

[Route("[controller]")]
[ApiController]
public class BreedController : ControllerBase
{
    private readonly IBreedRepository _breedRepository;
    private readonly ILogger<Breed> _logger;
    private readonly IMapper _mapper;

    public BreedController(IBreedRepository breedRepository, ILogger<Breed> logger, IMapper mapper)
    {
        _breedRepository = breedRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> CreateBreed([FromBody] BreedDto dataReceived)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for new breed with data {DataReceived}",
            nameof(CreateBreed), dataReceived);

        var recordToAdd = _mapper.Map<Breed>(dataReceived);

        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(
            instance: recordToAdd,
            validationContext: new ValidationContext(recordToAdd),
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

        bool alreadyExists = await _breedRepository.GetBreedByNameAsync(dataReceived.Name) != null;

        if (alreadyExists)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"The breed {dataReceived.Name} already exists."
            };

            return BadRequest(problemDetails);
        }

        await _breedRepository.CreateBreedAsync(recordToAdd);
        await _breedRepository.CommitChangesAsync();

        return CreatedAtAction(
            actionName: nameof(CreateBreed),
            routeValues: new { id = recordToAdd.Id },
            value: recordToAdd.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBreed([FromBody] BreedDto dataReceived)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for updating breed {id} with data {DataReceived}",
            nameof(UpdateBreed), dataReceived.Id, dataReceived);

        if (dataReceived.Id == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Invalid request.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The request is missing the breed id."
            };

            return BadRequest(problemDetails);
        }

        // Check if record exists
        var recordToUpdate = await _breedRepository.GetBreedByIdAsync(dataReceived.Id.Value);
        if (recordToUpdate == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The breed with id {dataReceived.Id} does not exist."
            };

            return NotFound(problemDetails);
        }

        _mapper.Map(dataReceived, recordToUpdate, typeof(BreedDto), typeof(Breed));

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

        bool alreadyExists = await _breedRepository.GetBreedByNameAsync(dataReceived.Name) != null;
        if (alreadyExists)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"The breed {dataReceived.Name} already exists."
            };

            return BadRequest(problemDetails);
        }

        _breedRepository.UpdateBreed(recordToUpdate);
        await _breedRepository.CommitChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteBreed([FromQuery] int id)
    {
        _logger.LogInformation("Begin call to {MethodName} for deleting breed {id}", nameof(DeleteBreed), id);

        var recordToDelete = await _breedRepository.GetBreedByIdAsync(id);

        // Check if record exists
        if (recordToDelete == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The breed with id {id} does not exist."
            };

            return NotFound(problemDetails);
        }

        //TODO: check if breed is used in animal repository

        _breedRepository.DeleteBreed(recordToDelete);
        await _breedRepository.CommitChangesAsync();

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BreedDto>>> GetBreeds()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetBreeds));

        IEnumerable<Breed> queryResults = await _breedRepository.GetBreedsSortedByNameAsync();
        List<BreedDto> breeds = _mapper.Map<List<BreedDto>>(queryResults);

        _logger.LogInformation("Returning {count} breeds.", breeds.Count);
        return Ok(breeds);
    }
}
