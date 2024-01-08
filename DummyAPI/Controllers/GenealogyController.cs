using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class GenealogyController : ControllerBase
{
    [HttpGet("Descendants", Name = "GetDescendants")]
    [SwaggerOperation(Summary = "Retrieves the descendants of a given animal")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns some basic information about the descendants", typeof(IEnumerable<DescendantDto>))]
    public async Task<ActionResult<IEnumerable<DescendantDto>>> GetDescendants(
        [FromQuery, SwaggerParameter("Animal ID", Required = true)] int animalId)
    {
        List<DescendantDto> listToReturn = new()
        {
            new DescendantDto()
            {
                ParentsCross = "Marquesa x Malhão",
                Id = 18,
                GenderId = 1,
                AnimalName = "PT 254 894658",
                IsActive = true
            },
            new DescendantDto()
            {
                ParentsCross = "PT 393 224973 x PT 879 254145",
                Id = 19,
                GenderId = 2,
                AnimalName = "",
                IsActive = false
            },
            new DescendantDto()
            {
                ParentsCross = "Marquesa x ?",
                Id = 20,
                GenderId = 1,
                AnimalName = "Airosa (PT 666 987412)",
                IsActive = true
            },
        };

        return listToReturn;
    }

}
