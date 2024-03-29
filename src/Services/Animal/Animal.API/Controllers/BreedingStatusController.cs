﻿using Animal.API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Animal.API.Controllers;

[Route("[controller]")]
[ApiController]
public class BreedingStatusController : ControllerBase
{
    private readonly ILogger<BreedingStatus> _logger;

    public BreedingStatusController(ILogger<BreedingStatus> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BreedingStatus>> GetBreedingStatus()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetBreedingStatus));

        var listToReturn = BreedingStatus.List();

        return Ok(listToReturn);
    }
}
