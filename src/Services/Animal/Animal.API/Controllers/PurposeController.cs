using Animal.API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Animal.API.Controllers;

[Route("[controller]")]
[ApiController]
public class PurposeController : ControllerBase
{
    private readonly ILogger<Purpose> _logger;

    public PurposeController(ILogger<Purpose> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Purpose>> GetPurposes()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetPurposes));

        var listToReturn = Enumeration.GetAll<Purpose>();

        return Ok(listToReturn);
    }
}
