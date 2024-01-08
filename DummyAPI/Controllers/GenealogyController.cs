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
        string DamLabel = "Marquesa";

        List<DescendantDto> listToReturn = new()
        {
            new DescendantDto()
            {
                DamLabel = DamLabel,
                SireLabel = "Malhão",
                Id = 18,
                GenderId = 1,
                DescendantLabel = "PT 254 894658",
                IsActive = true
            },
            new DescendantDto()
            {
                DamLabel = DamLabel,
                SireLabel = "PT 879 254145",
                Id = 19,
                GenderId = 2,
                DescendantLabel = "",
                IsActive = false
            },
            new DescendantDto()
            {
                DamLabel = DamLabel,
                SireLabel = "",
                Id = 20,
                GenderId = 1,
                DescendantLabel = "Airosa",
                IsActive = true
            },
        };

        return Ok(listToReturn);
    }


    [HttpGet("Ascendants", Name = "GetAscendants")]
    [SwaggerOperation(Summary = "Retrieves the ascendants of a given animal")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns some basic information about the ascendants", typeof(IEnumerable<AscendantDto>))]
    public async Task<ActionResult<IEnumerable<AscendantDto>>> GetAscendants(
        [FromQuery, SwaggerParameter("Animal ID", Required = true)] int animalId)
    {
        string name = "Marquesa";

        AscendantDto dtoToReturn = new()
        {
            Id = animalId,
            AscendantLabel = name,
            FeatureScore = "12.459 lt",
            Dam = new AscendantDto()
            {
                Id = 87,
                AscendantLabel = "Malhada",
                FeatureScore = "",
                Dam = new AscendantDto()
                {
                    Id = 66,
                    AscendantLabel = "PT 555 111 222",
                    FeatureScore = "9.864 lt"
                },
                Sire = new AscendantDto()
                {
                    Id = 156,
                    AscendantLabel = "PT 654 123321"
                }
            },
            Sire = new AscendantDto()
            {
                Id = 156,
                AscendantLabel = "Tsubasa",
                FeatureScore = "+1600 TPI",
                Sire = new AscendantDto()
                {
                    Id = 88,
                    AscendantLabel = "Rambo"
                }
            }
        };

        return Ok(dtoToReturn);
    }
}
