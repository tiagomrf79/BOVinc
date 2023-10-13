using AutoMapper;
using FarmsAPI.DTO;
using FarmsAPI.Models;
using FarmsAPI.Models.csv;

namespace FarmsAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Farm, FarmResponseDto>().ReverseMap();
        CreateMap<FarmUpdateDto, Farm>();
        CreateMap<FarmRecord, Farm>();
    }
}
