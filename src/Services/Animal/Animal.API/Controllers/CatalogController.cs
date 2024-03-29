﻿using Animal.API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Animal.API.Controllers;

[Route("[controller]")]
[ApiController]
public class CatalogController : ControllerBase
{
    private readonly ILogger<Catalog> _logger;

    public CatalogController(ILogger<Catalog> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Catalog>> GetCatalogs()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetCatalogs));

        var listToReturn = Catalog.List();
        
        return Ok(listToReturn);
    }
}
