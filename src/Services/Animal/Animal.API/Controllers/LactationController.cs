using Animal.API.DTOs;
using Animal.API.Infrastructure.Repositories;
using Animal.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Animal.API.Controllers;

[Route("[controller]")]
[ApiController]
public class LactationController : ControllerBase
{
    private readonly ILactationRepository _lactationRepository;
    private readonly ILogger<Lactation> _logger;
    private readonly IMapper _mapper;

    public LactationController(ILactationRepository lactationRepository, ILogger<Lactation> logger, IMapper mapper)
    {
        _lactationRepository = lactationRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LactationItemDto>>> GetLactationItems([FromQuery] int animalId)
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetLactationItems));

        //TODO: check if animal exists

        IEnumerable<Lactation> queryResults = await _lactationRepository.GetLactationsAsync(animalId);
        List<LactationItemDto> lactations = _mapper.Map<List<LactationItemDto>>(queryResults);

        _logger.LogInformation("Returning {count} lactations.", lactations.Count);
        return Ok(lactations);
    }

}
