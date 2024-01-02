﻿using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class MilkRecordController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<MilkRecordController> _logger;
    private readonly IDistributedCache _distributedCache;

    public MilkRecordController(ILogger<MilkRecordController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }

    [HttpGet("Table", Name = "GetMilkRecordsForTable")]
    [SwaggerOperation(Summary = "Gets data for a table with milk records for a given animal and lactation")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns calving date and a list of milk records", typeof(IEnumerable<MilkRecordForTableDto>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns a standard error response", typeof(ProblemDetails))]
    public async Task<ActionResult<IEnumerable<MilkRecordForTableDto>>> Get(
        [FromQuery, SwaggerParameter("Animal ID", Required = true)] int animalId,
        [FromQuery, SwaggerParameter("Lactation Number", Required = true)] int lactationId)
    {
        //each milk recording should reference an AnimalId and a LactationId
        //each time I add a calving or abortion(new lactation) it should create a record in the Lactations table
        //in case a cow is bought, the previous lactations won't exist because there are no calvings in her history
        //in case I want them to show I must insert the previous calvings and adjust the entered herd number of lactations
        //what happens when I delete a calving with milk recording in its lactation?
        //  prohibit deletion? move milk recordings to previous lactation? what if it's the first calving?
        //so I can query the Lactations table using LactationId to get the calving date


        if (animalId == 0)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The animal with ID {animalId} does not exist."
            };

            _logger.LogInformation("The animal with ID {id} does not exist.", animalId);

            return NotFound(problemDetails);
        }
        if (lactationId == 0)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Record not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The lactation with ID {lactationId} does not exist."
            };

            _logger.LogInformation("The lactation with ID {id} does not exist.", lactationId);

            return NotFound(problemDetails);
        }

        var milkRecords = new List<MilkRecordDto>
        {
            new MilkRecordDto()
            {
                Id = 1,
                Date = new DateOnly(2022,12,19),
                Milk = 30.65,
                Fat = 3.3,
                Protein = 5.9,
                SCC = 647,
                AnimalId = 1,
                LactationId = 1
            },
            new MilkRecordDto()
            {
                Id = 2,
                Date = new DateOnly(2023,1,2),
                Milk = 44.59,
                Fat = 4.9,
                Protein = 3.7,
                SCC = 457,
                AnimalId = 1,
                LactationId = 1
            },
            new MilkRecordDto()
            {
                Id = 3,
                Date = new DateOnly(2023,2,10),
                Milk = 49.81,
                Fat = 5.1,
                Protein = 5,
                SCC = 895,
                AnimalId = 1,
                LactationId = 1
            },
            new MilkRecordDto()
            {
                Id = 4,
                Date = new DateOnly(2023,3,18),
                Milk = 42.82,
                Fat = 2.3,
                Protein = 5,
                SCC = 442,
                AnimalId = 1,
                LactationId = 1
            },
            new MilkRecordDto()
            {
                Id = 5,
                Date = new DateOnly(2023,4,21),
                Milk = 42.89,
                Fat = 5.9,
                Protein = 3.4,
                SCC = 885,
                AnimalId = 1,
                LactationId = 1
            },
            new MilkRecordDto()
            {
                Id = 6,
                Date = new DateOnly(2023,5,3),
                Milk = 39.61,
                Fat = 3.8,
                Protein = 5.2,
                SCC = 586,
                AnimalId = 1,
                LactationId = 1
            },
            new MilkRecordDto()
            {
                Id = 7,
                Date = new DateOnly(2023,6,6),
                Milk = 31.9,
                Fat = 4.8,
                Protein = 5.3,
                SCC = 726,
                AnimalId = 1,
                LactationId = 1
            },
            new MilkRecordDto()
            {
                Id = 8,
                Date = new DateOnly(2023,7,13),
                Milk = 26,
                Fat = 4.1,
                Protein = 2.7,
                SCC = 677,
                AnimalId = 1,
                LactationId = 1
            }
        };

        var listToReturn = new List<MilkRecordForTableDto>()
        {
            new MilkRecordForTableDto()
            {
                CalvingDate = new DateOnly(2022,11,26),
                MilkRecords = milkRecords
            }
        };

        return Ok(listToReturn);
    }
    

    [HttpPost(Name = "CreateMilkRecord")]
    [SwaggerOperation(Summary = "Creates a new milk record")]
    [SwaggerResponse(StatusCodes.Status201Created, "Returns Created")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Returns a standard error response", typeof(ProblemDetails))]
    public async Task<ActionResult> Post(
        [FromBody, SwaggerRequestBody("Data to create milk record", Required = true)] MilkRecordDto dtoReceived)
    {
        dtoReceived.Id = 154;

        return CreatedAtAction(nameof(Post), new { id = dtoReceived.Id }, dtoReceived);
        //return Ok();
    }


    [HttpPut(Name = "UpdateMilkRecord")]
    [SwaggerOperation(Summary = "Updates a milk record")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns OK")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Returns a standard error response", typeof(ProblemDetails))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns a standard error response", typeof(ProblemDetails))]
    public async Task<ActionResult> Put(
        [FromBody, SwaggerRequestBody("Data to update milk record", Required = true)] MilkRecordDto dtoReceived)
    {
        return Ok();
    }

    [HttpDelete(Name = "DeleteMilkRecord")]
    [SwaggerOperation(Summary = "Deletes a milk record")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Returns NoContent")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Returns a standard error response", typeof(ProblemDetails))]
    public async Task<ActionResult> Delete(
        [FromQuery, SwaggerParameter("Milk Record ID", Required = true)] int milkRecordId)
    {
        return NoContent();
    }


}
