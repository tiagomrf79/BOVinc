using Animal.API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Animal.API.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ILogger _logger;

    public CategoryController(ILogger logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetCategories));

        var listToReturn = Enumeration.GetAll<Category>();

        return Ok(listToReturn);
    }
}
