using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Occurrence.API.DTOs;
using Occurrence.API.Enums;
using Occurrence.API.Infrastructure.Repositories;
using Occurrence.API.Models;

namespace Occurrence.API.Controllers;

[Route("[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventRepository _eventRepository;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public EventController(IEventRepository eventRepository, ILogger logger, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents([FromQuery] int animalId)
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetEvents));

        //TODO: check if animal exists

        IEnumerable<Event> queryResults = await _eventRepository.GetEventsSortedByDateAsync(animalId);
        List<EventDto> events = _mapper.Map<List<EventDto>>(queryResults);

        _logger.LogInformation("Returning {count} events.", events.Count);
        return Ok(events);
    }

    [HttpGet("types")]
    public ActionResult<IEnumerable<EventType>> GetEventTypes()
    {
        _logger.LogInformation("Begin call to {MethodName}", nameof(GetEventTypes));

        var listToReturn = EventType.List();

        return Ok(listToReturn);
    }
}
