using AutoMapper;
using Occurrence.API.DTOs;
using Occurrence.API.Models;

namespace Occurrence.API.Mappings;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<Event, EventDto>();
    }
}
