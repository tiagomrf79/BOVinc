﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Production.API.DTOs;
using Production.API.Enums;
using Production.API.Infrastructure;
using Production.API.Models;
using System.ComponentModel.DataAnnotations;

namespace Production.API.Controllers;

[Route("[controller]")]
[ApiController]
public class TestSampleController : ControllerBase
{
    private readonly ProductionContext _productionContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public TestSampleController(ProductionContext context, ILogger logger, IMapper mapper)
    {
        _productionContext = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> CreateTestSample([FromBody] FullSampleDto dataReceived)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for new test sample with data {DataReceived}",
            nameof(CreateTestSample), dataReceived);

        var recordToAdd = _mapper.Map<TestSample>(dataReceived);

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

        Lactation? lactation = await GetLactationByDate(dataReceived.AnimalId, dataReceived.Date);

        // Date of test sample must be between a calving date and end date
        if (lactation == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Data of test sample is not valid for this animal."
            };

            return BadRequest(problemDetails);
        }

        bool alreadyExists = await _productionContext.TestSamples
            .Where(x =>
                x.AnimalId == dataReceived.AnimalId
                && x.Date == dataReceived.Date
                && x.MilkYield == dataReceived.MilkYield
                && x.FatPercentage == dataReceived.FatPercentage
                && x.ProteinPercentage == dataReceived.ProteinPercentage
                && x.SomaticCellCount == dataReceived.SomaticCellCount
            ).AnyAsync();

        // Don't allow duplicate measurements (same animal, date, milk, fat, protein and SCC)
        if (alreadyExists)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"A test sample with the same values already exists."
            };

            return BadRequest(problemDetails);
        }

        recordToAdd.CreatedAt = DateTime.UtcNow;
        recordToAdd.LastUpdatedAt = recordToAdd.CreatedAt;
        await _productionContext.TestSamples.AddAsync(recordToAdd);
        await _productionContext.SaveChangesAsync();

        return CreatedAtAction(
            actionName: nameof(CreateTestSample),
            routeValues: new { id = recordToAdd.Id },
            value: recordToAdd.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateTestSample([FromBody] FullSampleDto dataReceived)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for updating test sample {id} with data {DataReceived}",
            nameof(UpdateTestSample), dataReceived.Id , dataReceived);

        var recordToUpdate = await _productionContext.TestSamples.FindAsync(dataReceived.Id);
        
        // Check if record exists
        if (recordToUpdate == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The test sample with id {dataReceived.Id} does not exist."
            };

            return NotFound(problemDetails);
        }

        _mapper.Map(dataReceived, recordToUpdate, typeof(FullSampleDto), typeof(TestSample));

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

        Lactation? lactation = await GetLactationByDate(dataReceived.AnimalId, dataReceived.Date);

        // Date of test sample must be between a calving date and end date
        if (lactation == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Data of test sample is not valid for this animal."
            };

            return BadRequest(problemDetails);
        }

        bool alreadyExists = await _productionContext.TestSamples
            .Where(x =>
                x.Id != dataReceived.Id
                && x.AnimalId == dataReceived.AnimalId
                && x.Date == dataReceived.Date
                && x.MilkYield == dataReceived.MilkYield
                && x.FatPercentage == dataReceived.FatPercentage
                && x.ProteinPercentage == dataReceived.ProteinPercentage
                && x.SomaticCellCount == dataReceived.SomaticCellCount
            ).AnyAsync();

        // Don't allow duplicate measurements (same animal, date, milk, fat, protein and SCC)
        if (alreadyExists)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"A test sample with the same values already exists."
            };

            return BadRequest(problemDetails);
        }

        recordToUpdate.LastUpdatedAt = DateTime.UtcNow;
        _productionContext.Update(recordToUpdate);
        await _productionContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteTestSample([FromQuery] int id)
    {
        _logger.LogInformation("Begin call to {MethodName} for deleting test sample {id}", nameof(DeleteTestSample), id);

        var recordToDelete = await _productionContext.TestSamples.FindAsync(id);

        // Check if record exists
        if (recordToDelete == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The test sample with id {id} does not exist."
            };

            return NotFound(problemDetails);
        }

        _productionContext.Remove(recordToDelete);
        await _productionContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<TestSamplesForTableVm>> GetTestSamplesForTable(
        [FromQuery] int animalId,
        [FromQuery] int lactationId)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for animal id {AnimalId} and lactation id {LactationId}",
            nameof(GetTestSamplesForTable), animalId, lactationId);

        List<FullSampleDto> testSamples = new();

        Lactation? lactation = await GetLactationById(lactationId);
        if (lactation == null)
        {
            _logger.LogInformation("Lactation with id {id} was not found.", lactationId);
            return Ok(
                new TestSamplesForTableVm(null, testSamples));
        }

        List<TestSample> queryResults = await _productionContext.TestSamples
            .Where(x =>
                x.AnimalId == animalId
                && x.Date >= lactation.CalvingDate
                && lactation.EndDate == null || x.Date < lactation.EndDate)
            .ToListAsync();

        testSamples = _mapper.Map<List<FullSampleDto>>(queryResults);

        TestSamplesForTableVm dtoToReturn = new(
            CalvingDate: lactation.CalvingDate,
            TestSamples: testSamples);

        _logger.LogInformation("Returning {count} test samples.", testSamples.Count);
        return Ok(dtoToReturn);
    }

    [HttpGet("Chart")]
    public async Task<ActionResult<TestSamplesForChartVm>> GetTestSamplesForChart(
        [FromQuery] int animalId,
        [FromQuery] int lactationId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("Total")]
    public async Task<ActionResult<IEnumerable<TotalsForTableVm>>> GetMilkTotalsForTable(
        [FromQuery] int animalId,
        [FromQuery] int lactationId)
    {
        throw new NotImplementedException();
    }

    private async Task<Lactation?> GetLactationByDate(int animalId, DateOnly date)
    {
        var lactations = _productionContext.Lactations.Where(x => x.AnimalId == animalId);

        // get latest lactation that starts before the given date
        Lactation? currentLactation = await lactations
            .Where(x => x.CalvingDate <= date)
            .OrderByDescending(x => x.CalvingDate)
            .FirstOrDefaultAsync();

        // no match for given date
        if (currentLactation == null)
            return null;

        currentLactation.EndDate = await GetEndDateForLactation(currentLactation);

        return currentLactation;
    }

    private async Task<Lactation?> GetLactationById(int lactationId)
    {
        Lactation? currentLactation = await _productionContext.Lactations.FindAsync(lactationId);

        // no match for given id
        if (currentLactation == null)
            return null;

        currentLactation.EndDate = await GetEndDateForLactation(currentLactation);

        return currentLactation;
    }

    private async Task<DateOnly?> GetEndDateForLactation(Lactation lactation)
    {
        if (lactation.EndDate != null)
            return lactation.EndDate;

        Lactation? nextLactation = await _productionContext.Lactations
            .Where(x =>
                x.AnimalId == lactation.AnimalId
                && x.CalvingDate > lactation.CalvingDate)
            .OrderBy(x => x.CalvingDate)
            .FirstOrDefaultAsync();

        // ongoing lactation => no end date
        if (nextLactation == null)
            return null;

        // closed lactation => next lactation calving date
        return nextLactation.CalvingDate;
    }
}

//  get all measurements and calving date for a given animal and lactation: GET recordings
//  insert one measurement: POST recordings
//  update one measurement: PUT recordings/1
//  delete one measurement: DELETE recordings/1
//get all measurements and all predictions and calving date for a given animal and lactation: GET recordings/chart
//get measurement total and prediction total for a given animal and lactation: GET recordings/total
 