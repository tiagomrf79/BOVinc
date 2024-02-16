using Animal.API.DTOs;
using Animal.API.Models;
using AutoMapper;

namespace Animal.API.Mappings;

public class LactationProfile : Profile
{
    public LactationProfile()
    {
        CreateMap<Lactation, LactationItemDto>();
    }
}
