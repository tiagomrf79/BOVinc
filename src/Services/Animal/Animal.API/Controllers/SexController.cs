using Animal.API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Animal.API.Controllers;

[Route("[controller]")]
[ApiController]
public class SexController : ControllerBase
{
    private readonly ILogger<Sex> _logger;

    public SexController(ILogger<Sex> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Sex>> GetSex()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetSex));

        var listToReturn = Enumeration.GetAll<Sex>();

        return Ok(listToReturn);
    }
}
