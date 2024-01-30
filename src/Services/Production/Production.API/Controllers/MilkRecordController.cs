using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Production.API.DTOs;
using Production.API.Infrastructure;
using Production.API.Models;

namespace Production.API.Controllers;

[Route("[controller]")]
[ApiController]
public class MilkRecordController : ControllerBase
{
    private readonly ProductionContext _productionContext;
    private readonly ILogger _logger;

    public MilkRecordController(ProductionContext context, ILogger logger)
    {
        _productionContext = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<MilkRecordForTableDto>> GetMilkRecordsForTable(
        [FromQuery] int animalId,
        [FromQuery] int lactationId)
    {
        _logger.LogInformation(
            "Begin call to {MethodName} for animal id {AnimalId} and lactation id {LactationId}",
            nameof(GetMilkRecordsForTable), animalId, lactationId);

        Lactation? lactation = await _productionContext.Lactations.FindAsync(lactationId);

        if (lactation == null)
        {
            _logger.LogInformation("Lactation was not found, returning empty list.");
            return Ok(new List<MilkRecordForTableDto>());
        }

        if (lactation.EndDate == null)
        {
            //get next lactation for this animal, if any
            //get its calving date
            //set current lactation end date with this calving date
        }

        _productionContext.MilkRecords.Where(x => 
            x.AnimalId == animalId 
            && x.Date >= lactation.CalvingDate 
            && lactation.EndDate == null || x.Date < lactation.EndDate);
    }
}

//get all measurements and calving date for a given animal and lactation: GET recordings
//insert one measurement: POST recordings
//update one measurement: PUT recordings/1
//delete one measurement: DELETE recordings/1
//get all measurements and all predictions and calving date for a given animal and lactation: GET recordings/adjusted
//get measurement totals and prediction totals for a given animal and lactation: GET recordings/totals
