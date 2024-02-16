using Microsoft.AspNetCore.Mvc;
using Occurrence.API.Enums;

namespace Occurrence.API.Controllers;

[Route("[controller]")]
[ApiController]
public class MovementController : ControllerBase
{
    private readonly ILogger _logger;

    public MovementController(ILogger logger)
    {
        _logger = logger;
    }

    [HttpGet("ReasonsToEnter")]
    public ActionResult<IEnumerable<ReasonEnteredHerd>> GetReasonsToEnterHerd()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetReasonsToEnterHerd));

        var listToReturn = ReasonEnteredHerd.List();

        return Ok(listToReturn);
    }

    [HttpGet("ReasonsToLeave")]
    public ActionResult<IEnumerable<ReasonLeftHerd>> GetReasonsToLeaveHerd()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetReasonsToLeaveHerd));

        var listToReturn = ReasonLeftHerd.List();

        return Ok(listToReturn);
    }
}
