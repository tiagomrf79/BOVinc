using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Production.API.DTOs;
using Production.API.Infrastructure;
using Production.API.Models;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Production.API.Controllers;

[Route("[controller]")]
[ApiController]
public class MilkRecordController : ControllerBase
{
    private readonly ProductionContext _productionContext;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public MilkRecordController(ProductionContext context, ILogger logger, IMapper mapper)
    {
        _productionContext = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
        _mapper = mapper;
    }

    //Milk measurements are assigned to the given lactation at runtime

    [HttpPost]
    public async Task<ActionResult> CreateMilkRecord([FromBody] MilkRecordDto dataReceived)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for new milk record with data {DataReceived}",
            nameof(CreateMilkRecord), dataReceived);

        var recordToAdd = _mapper.Map<MilkRecord>(dataReceived);

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

        // Date of milk record must be between a calving date and end date
        if (lactation == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Data of milk record is not valid for this animal."
            };

            return BadRequest(problemDetails);
        }

        bool alreadyExists = await _productionContext.MilkRecords
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
                Detail = $"A milk record with the same values already exists."
            };

            return BadRequest(problemDetails);
        }

        recordToAdd.CreatedAt = DateTime.UtcNow;
        recordToAdd.LastUpdatedAt = recordToAdd.CreatedAt;
        await _productionContext.MilkRecords.AddAsync(recordToAdd);
        await _productionContext.SaveChangesAsync();

        return CreatedAtAction(
            actionName: nameof(CreateMilkRecord),
            routeValues: new { id = recordToAdd.Id },
            value: recordToAdd.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateMilkRecord([FromBody] MilkRecordDto dataReceived)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for updating milk record {id} with data {DataReceived}",
            nameof(UpdateMilkRecord), dataReceived.Id , dataReceived);

        var recordToUpdate = await _productionContext.MilkRecords.FindAsync(dataReceived.Id);
        
        // Check if record exists
        if (recordToUpdate == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The milk record with id {dataReceived.Id} does not exist."
            };

            return NotFound(problemDetails);
        }

        _mapper.Map(dataReceived, recordToUpdate, typeof(MilkRecordDto), typeof(MilkRecord));

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

        // Date of milk record must be between a calving date and end date
        if (lactation == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Invalid request.",
                Status = StatusCodes.Status400BadRequest,
                Detail = $"Data of milk record is not valid for this animal."
            };

            return BadRequest(problemDetails);
        }

        bool alreadyExists = await _productionContext.MilkRecords
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
                Detail = $"A milk record with the same values already exists."
            };

            return BadRequest(problemDetails);
        }

        recordToUpdate.LastUpdatedAt = DateTime.UtcNow;
        _productionContext.Update(recordToUpdate);
        await _productionContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteMilkRecord([FromQuery] int id)
    {
        _logger.LogInformation("Begin call to {MethodName} for deleting milk record {id}", nameof(DeleteMilkRecord), id);

        var recordToDelete = await _productionContext.MilkRecords.FindAsync(id);

        // Check if record exists
        if (recordToDelete == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The milk record with id {id} does not exist."
            };

            return NotFound(problemDetails);
        }

        _productionContext.Remove(recordToDelete);
        await _productionContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<MilkRecordForTableDto>> GetMilkRecordsForTable(
        [FromQuery] int animalId,
        [FromQuery] int lactationId)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for animal id {AnimalId} and lactation id {LactationId}",
            nameof(GetMilkRecordsForTable), animalId, lactationId);

        List<MilkRecordDto> milkRecords = new();

        Lactation? lactation = await GetLactationById(lactationId);
        if (lactation == null)
        {
            _logger.LogInformation("Lactation with id {id} was not found.", lactationId);
            return Ok(
                new MilkRecordForTableDto(null, milkRecords));
        }

        List<MilkRecord> queryResults = await _productionContext.MilkRecords
            .Where(x =>
                x.AnimalId == animalId
                && x.Date >= lactation.CalvingDate
                && lactation.EndDate == null || x.Date < lactation.EndDate)
            .ToListAsync();

        milkRecords = _mapper.Map<List<MilkRecordDto>>(queryResults);

        MilkRecordForTableDto dtoToReturn = new(
            CalvingDate: lactation.CalvingDate,
            MilkRecords: milkRecords);

        _logger.LogInformation("Returning {milkRecords.Count} milk records.", milkRecords.Count);
        return Ok(dtoToReturn);
    }

    [HttpGet("Chart")]
    public async Task<ActionResult<MilkRecordForChartDto>> GetMilkRecordsForChart(
        [FromQuery] int animalId,
        [FromQuery] int lactationId)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for animal id {AnimalId} and lactation id {LactationId}",
            nameof(GetMilkRecordsForChart), animalId, lactationId);

        List<MilkOnlyRecordDto> actualMilkRecords = new();
        List<MilkOnlyRecordDto> adjustedMilkRecords = new();

        Lactation? lactation = await GetLactationById(lactationId);
        if (lactation == null)
        {
            _logger.LogInformation("Lactation with id {id} was not found.", lactationId);
            return Ok(
                new MilkRecordForChartDto(null, actualMilkRecords, adjustedMilkRecords));
        }

        List<MilkRecord> queryResults = await _productionContext.MilkRecords
            .Where(x =>
                x.AnimalId == animalId
                && x.Date >= lactation.CalvingDate
                && lactation.EndDate == null || x.Date < lactation.EndDate)
            .ToListAsync();

        actualMilkRecords = _mapper.Map<List<MilkOnlyRecordDto>>(queryResults);

        throw new NotImplementedException();

        MilkRecordForChartDto dtoToReturn = new(
            lactation.CalvingDate,
            actualMilkRecords,
            adjustedMilkRecords);

        return Ok(dtoToReturn);
    }

    [HttpGet("Total")]
    public async Task<ActionResult<MilkTotalForTableDto>> GetMilkTotalsForTable(
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
//get all measurements and all predictions and calving date for a given animal and lactation: GET recordings/adjusted
//get measurement totals and prediction totals for a given animal and lactation: GET recordings/totals
