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
    [SwaggerResponse(StatusCodes.Status200OK, "Returns some basic information about the ascendants", typeof(AscendantDto))]
    public async Task<ActionResult<AscendantDto>> GetAscendants(
        [FromQuery, SwaggerParameter("Animal ID", Required = true)] int animalId)
    {
        string name = "Marquesa";

        AscendantDto dtoToReturn = new()
        {
            AnimalLabel = name,
            Parents = new List<ParentDto>()
            {
                new ParentDto()
                {
                    Id = 87,
                    GenderId = 1,
                    ParentLabel = "Malhada",
                    ParentScore = "",
                    GrandParents = new List<GrandParentDto>()
                    {
                        new GrandParentDto()
                        {
                            Id = 66,
                            GenderId = 1,
                            GrandParentLabel = "PT 555 111 222",
                            GrandParentScore = "9.864 lt"
                        },
                        new GrandParentDto()
                        {
                            Id = 156,
                            GenderId = 2,
                            GrandParentLabel = "PT 654 123321"
                        }
                    }
                },
                new ParentDto()
                {
                    Id = 156,
                    GenderId = 2,
                    ParentLabel = "Tsubasa",
                    ParentScore = "+1600 TPI",
                    GrandParents = new List<GrandParentDto>()
                    {
                        new GrandParentDto()
                        {
                            Id = 88,
                            GenderId = 2,
                            GrandParentLabel = "Rambo"
                        }
                    }
                }
            }

        };

        return Ok(dtoToReturn);
    }
}
