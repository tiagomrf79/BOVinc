﻿using DummyAPI.DTOs;
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

        return listToReturn;
    }

}
