using AnimalsAPI.DTOs;
using AnimalsAPI.Models;
using AnimalsAPI.Models.csv;
using AutoMapper;

namespace AnimalsAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Animal, AnimalResponseDto>().ReverseMap();
        CreateMap<AnimalUpdateDto, Animal>();
        CreateMap<AnimalRecord, Animal>();
    }
}
