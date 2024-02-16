using Animal.API.DTOs;
using Animal.API.Models;
using AutoMapper;

namespace Animal.API.Mappings;

public class AnimalProfile : Profile
{
    public AnimalProfile()
    {
        CreateMap<AnimalUpdateDto, FarmAnimal>();
        CreateMap<FarmAnimal, AnimalDetailDto>()
            .ForMember(
                dest => dest.LastLactationNumber,
                opt => opt.MapFrom(
                    (src, dest, destMember, context) => context.Items["LastLactationNumber"]));
        CreateMap<FarmAnimal, CalfDto>();
        CreateMap<FarmAnimal, HeiferDto>();
    }
}
