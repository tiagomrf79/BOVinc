using AutoMapper;
using HerdsAPI.DTO;
using HerdsAPI.Models;

namespace HerdsAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Herd, HerdDto>().ReverseMap();
        CreateMap<CreateHerdDto, Herd>();
    }
}
