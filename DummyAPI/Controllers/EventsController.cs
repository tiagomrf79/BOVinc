using DummyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;

namespace DummyAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class EventsController : ControllerBase
{
    private static readonly SemaphoreSlim semaphore = new(1, 1);

    private readonly ILogger<AnimalController> _logger;
    private readonly IDistributedCache _distributedCache;

    public EventsController(ILogger<AnimalController> logger, IDistributedCache distributedCache)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }


    [HttpGet("Dropdown", Name = "GetEventTypesForDropdown")]
    [SwaggerOperation(Summary = "Gets event type options for a dropdown")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of event type options", typeof(IEnumerable<OptionForDropdownDto>))]
    public async Task<ActionResult<IEnumerable<OptionForDropdownDto>>> Get()
    {
        var listToReturn = new List<OptionForDropdownDto>()
        {
            new OptionForDropdownDto { Id = 1, Name = "Born" },
            new OptionForDropdownDto { Id = 2, Name = "Entered Herd" },
            new OptionForDropdownDto { Id = 3, Name = "Prostaglandin" },
            new OptionForDropdownDto { Id = 4, Name = "Heat" },
            new OptionForDropdownDto { Id = 5, Name = "Breeding" },
            new OptionForDropdownDto { Id = 6, Name = "Negative Pregnancy" },
            new OptionForDropdownDto { Id = 7, Name = "Confirmed Pregnant" },
            new OptionForDropdownDto { Id = 8, Name = "Dry Off" },
            new OptionForDropdownDto { Id = 9, Name = "Abortion" },
            new OptionForDropdownDto { Id = 10, Name = "Abortion (New Lactation)" },
            new OptionForDropdownDto { Id = 11, Name = "Calving" },
            new OptionForDropdownDto { Id = 12, Name = "Weaning" },
            new OptionForDropdownDto { Id = 13, Name = "Vaccination" },
            new OptionForDropdownDto { Id = 14, Name = "Diagnosis" },
            new OptionForDropdownDto { Id = 15, Name = "Changed Group" },
            new OptionForDropdownDto { Id = 16, Name = "Left Herd" },
            new OptionForDropdownDto { Id = 17, Name = "Died" },
        };

        return Ok(listToReturn);
    }


    [HttpGet("EventsForAnimal", Name = "GetEventsForAnimal")]
    [SwaggerOperation(Summary = "Retrieves a list of events and reminders for a given animal.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of events and reminders", typeof(IEnumerable<EventReminderDto>))]
    public async Task<ActionResult<IEnumerable<EventReminderDto>>> GetEventsForAnimal(
        [FromQuery, SwaggerParameter("Animal ID", Required = true)] int animalId)
    {
        List<EventReminderDto> listToReturn = new()
        {
            new EventReminderDto()
            {
                IsReminder = true,
                ReminderTypeId = 1,
                ReminderType = "Possible Heat",
                Date = new DateOnly(2023,7,29)
            },
            new EventReminderDto()
            {
                IsReminder = true,
                ReminderTypeId = 1,
                ReminderType = "Possible Heat",
                Date = new DateOnly(2023,7,8)
            },
            new EventReminderDto()
            {
                Id = 1,
                Date = new DateOnly(2022,6,17),
                EventTypeId = 5,
                EventType = "Breeding",
                IsBreedingBullKnown = true,
                IsCatalogSire = true,
                BreedingBullId = 459,
                BreedingBull = "Tsubasa - CANM49887946546",
                EventDescription = "Artificial Insemination with bull Tsubasa.",
                Notes = "Insemination was done by farm technician."
            },
            new EventReminderDto()
            {
                Id = 2,
                Date = new DateOnly(2022,5,22),
                EventTypeId = 15,
                EventType = "Changed Group",
                PreviousGroupId = 4,
                PreviousGroup = "Younger Heifers",
                NewGroupId = 3,
                NewGroup = "Older Heifers",
                EventDescription = "Move from younger heifers group to older heifers group."
            },
            new EventReminderDto()
            {
                Id = 3,
                Date = new DateOnly(2022,1,1),
                EventTypeId = 2,
                EventType = "Entered Herd",
                ReasonForEnteringId = 3,
                ReasonForEntering = "Bought",
                EventDescription = "Bought",
                Notes = "Cost me the eyes of the face."
            }
        };

        return Ok(listToReturn);
    }
}
