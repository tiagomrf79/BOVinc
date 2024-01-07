using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AnimalStatusController : ControllerBase
{

    [HttpGet("AnimalStatus", Name = "GetAnimalStatus")]
    [SwaggerOperation(Summary = "Retrieves status details, for a given animal")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns animal status details", typeof(AnimalStatusDto))]
    public async Task<ActionResult<AnimalStatusDto>> GetAnimalStatus(
        [FromQuery, SwaggerParameter("Animal ID", Required = true)] int animalId)
    {
        if (animalId == 1)
        {
            return new AnimalStatusDto
            {
                Active = true,
                DateOfBirth = new DateOnly(2016, 8, 28),
                CurrentGroupName = "Main barn",
                LactationNumber = 5,
                MilkingStatusId = 1,
                MilkingStatus = "Milking",
                LastCalvingDate = new DateOnly(2023, 12, 26),
                BreedingStatusId = 1,
                BreedingStatus = "Open",
                LastHeatDate = new DateOnly(2023,12,30),
                ExpectedHeatDate = new DateOnly(2024, 1, 19),
            };
        }
        else if (animalId == 2)
        {
            return new AnimalStatusDto
            {
                Active = true,
                DateOfBirth = new DateOnly(2012, 7, 29),
                CurrentGroupName = "Maternity pen",
                LactationNumber = 3,
                MilkingStatusId = 2,
                MilkingStatus = "Dry",
                DryDate = new DateOnly(2023, 11, 11),
                BreedingStatusId = 3,
                BreedingStatus = "Confirmed",
                LastBreedingDate = new DateOnly(2023,2,21),
                DueDate = new DateOnly(2024,1,11)
            };
        }
        else if (animalId == 59)
        {
            return new AnimalStatusDto
            {
                Active = false,
                LactationNumber = 8,
            };
        }
        else if (animalId == 13)
        {
            return new AnimalStatusDto
            {
                Active = true,
                DateOfBirth = new DateOnly(2021, 8, 5),
                CurrentGroupName = "Late heifers",
                BreedingStatusId= 1,
                BreedingStatus = "Open",
            };
        }
        else if (animalId == 9)
        {
            return new AnimalStatusDto
            {
                Active = true,
                DateOfBirth = new DateOnly(2022, 8, 8),
                LactationNumber = 0,
                BreedingStatusId = 2,
                BreedingStatus = "Bred",
                LastBreedingDate = new DateOnly(2023, 11, 10),
            };
        }
        else return BadRequest();
    }


}
