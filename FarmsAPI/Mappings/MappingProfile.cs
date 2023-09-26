using AutoMapper;
using FarmsAPI.DTO;
using FarmsAPI.Models;

namespace FarmsAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Herd, HerdDto>().ReverseMap();
        CreateMap<CreateHerdDto, Herd>();
    }
}
