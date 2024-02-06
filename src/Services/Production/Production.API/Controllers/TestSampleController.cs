﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Production.API.DTOs;
using Production.API.Infrastructure.Repositories;
using Production.API.Models;
using Production.API.Services;
using System.ComponentModel.DataAnnotations;

namespace Production.API.Controllers;

[Route("[controller]")]
[ApiController]
public class TestSampleController : ControllerBase
{
    private readonly ITestSampleRepository _testSampleRepository;
    private readonly ILactationRepository _lactationRepository;
    private readonly ILactationService _lactationService;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public TestSampleController(
        ITestSampleRepository testSampleRepository,
        ILactationRepository lactationRepository,
        ILactationService lactationService,
        ILogger logger, IMapper mapper)
    {
        _testSampleRepository = testSampleRepository;
        _lactationRepository = lactationRepository;
        _lactationService = lactationService;
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

        bool alreadyExists = await _testSampleRepository.IsTestSampleDuplicatedAsync(dataReceived.AnimalId, dataReceived.Date, dataReceived.MilkYield);

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

        await _testSampleRepository.CreateTestSampleAsync(recordToAdd);
        await _testSampleRepository.CommitChangesAsync();

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

        if (dataReceived.Id == null)
        {
            ProblemDetails problemDetails = new()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Invalid request.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The request is missing the test sample id."
            };

            return BadRequest(problemDetails);
        }

        var recordToUpdate = await _testSampleRepository.GetTestSampleByIdAsync((int)dataReceived.Id);

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

        bool alreadyExists = await _testSampleRepository.IsTestSampleDuplicatedAsync(
            dataReceived.AnimalId, dataReceived.Date, dataReceived.MilkYield, dataReceived.Id);

        // Don't allow duplicate measurements (same animal, date and milk)
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

        _testSampleRepository.UpdateTestSample(recordToUpdate);
        await _testSampleRepository.CommitChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteTestSample([FromQuery] int id)
    {
        _logger.LogInformation("Begin call to {MethodName} for deleting test sample {id}", nameof(DeleteTestSample), id);

        var recordToDelete = await _testSampleRepository.GetTestSampleByIdAsync(id);

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

        _testSampleRepository.DeleteTestSample(recordToDelete);
        await _testSampleRepository.CommitChangesAsync();

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
            return Ok(new TestSamplesForTableVm(null, testSamples));
        }

        IEnumerable<TestSample> queryResults = await _testSampleRepository.GetSortedTestSamplesForPeriodAsync(animalId, lactation.CalvingDate, lactation.EndDate);

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
        _logger.LogInformation(
            "Begin call to {MethodName} for animal id {AnimalId} and lactation id {LactationId}",
            nameof(GetMilkTotalsForTable), animalId, lactationId);

        Lactation? lactation = await GetLactationById(lactationId);
        if (lactation == null)
        {
            _logger.LogInformation("Lactation with id {id} was not found.", lactationId);
            return Ok(new List<TotalsForTableVm>());
        }

        // get the test-day samples for the given animal and lactation
        IEnumerable<TestSample> testDaySamples = await _testSampleRepository.GetSortedTestSamplesForPeriodAsync(animalId, lactation.CalvingDate, lactation.EndDate);

        // transform test-day samples into records expected by lactation service
        List<YieldRecordDto> milkSamples = testDaySamples
            .Select(x => new YieldRecordDto(x.Date.DayNumber - lactation.CalvingDate.DayNumber, x.MilkYield))
            .ToList();

        // feed records into lactation service and get milk yields per interval
        List<IntervalYieldDto> milkYields = await _lactationService.GetAdjustedMilkYields(milkSamples, lactation.Number);

        #region MaybeRemove
        // join milk yield to its sample (to use for solid yield calculations)
        var interm = testDaySamples
            .Zip(milkSamples, (x,y) =>
                new
                {
                    x.Date,
                    x.FatPercentage,
                    x.ProteinPercentage,
                    IntervalYield = y.Yield
                })
            .ToList();

        // transform test-day samples with milk yields into records with fat and protein yields
        List<YieldRecordDto> fatSamples = new List<YieldRecordDto>();
        List<YieldRecordDto> proteinSamples = new List<YieldRecordDto>();

        double milkYieldForFat = 0;
        double milkYieldForProtein = 0;

        foreach (var item in interm)
        {
            double milkYield = item.IntervalYield;
            milkYieldForFat += milkYield;
            milkYieldForProtein += milkYield;

            int daysInMilk = item.Date.DayNumber - lactation.CalvingDate.DayNumber;

            if (item.FatPercentage != null)
            {
                fatSamples.Add(new YieldRecordDto(
                    daysInMilk,
                    milkYieldForFat * item.FatPercentage.Value / 100
                ));
            }

            if (item.ProteinPercentage != null)
            {
                proteinSamples.Add(new YieldRecordDto(
                    daysInMilk,
                    milkYieldForProtein * item.ProteinPercentage.Value / 100
                ));
            }
        }
        #endregion

        // feed fat records into lactation service and get fat yields per interval
        List<IntervalYieldDto> fatYields = await _lactationService.GetAdjustedFatYields(fatSamples, lactation.Number);

        // feed protein records into lactation service and get protein yields per interval
        List<IntervalYieldDto> proteinYields = await _lactationService.GetAdjustedProteinYields(proteinSamples, lactation.Number);

        // calculate total yield during the lactation
        int milkTotal = (int)Math.Round(
            milkYields.Sum(x => (x.DaysInMilkAtEnd - x.DaysInMilkAtStart) * x.Yield),
            MidpointRounding.AwayFromZero);
        int fatTotal = (int)Math.Round(
            fatYields.Sum(x => (x.DaysInMilkAtEnd - x.DaysInMilkAtStart) * x.Yield),
            MidpointRounding.AwayFromZero);
        int proteinTotal = (int)Math.Round(
            proteinYields.Sum(x => (x.DaysInMilkAtEnd - x.DaysInMilkAtStart) * x.Yield),
            MidpointRounding.AwayFromZero);

        var actualTotals = new List<TotalDto>() {
            new TotalDto(
                MilkYield: milkTotal,
                FatYield: fatTotal,
                ProteinYield: proteinTotal)};

        var adjustedTotals = new List<TotalDto>() {
            new TotalDto(
                MilkYield: adjustedYields.Item1,
                FatYield: adjustedYields.Item2,
                ProteinYield: adjustedYields.Item3)};

        var listToReturn = new List<TotalsForTableVm>()
        {
            new TotalsForTableVm(Order: 1, Name: "Actual production", Totals: actualTotals),
            new TotalsForTableVm(Order: 2, Name: "Adjusted to 305 days", Totals: adjustedTotals)
        };

        return Ok(listToReturn);
    }

    private async Task<Lactation?> GetLactationByDate(int animalId, DateOnly date)
    {
        Lactation? currentLactation = await _lactationRepository.GetLactationByDateAsync(animalId, date);

        // no match for given date
        if (currentLactation == null)
            return null;

        currentLactation.EndDate = await GetEndDateForLactation(currentLactation);

        return currentLactation;
    }

    private async Task<Lactation?> GetLactationById(int lactationId)
    {
        Lactation? currentLactation = await _lactationRepository.GetLactationByIdAsync(lactationId);

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

        Lactation? nextLactation = await _lactationRepository.GetSubsequentLactationAsync(lactation);

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
 