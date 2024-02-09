using Animal.API.DTOs;
using Animal.API.Models;
using AutoMapper;

namespace Animal.API.Mappings;

public class BreedProfile : Profile
{
    public BreedProfile()
    {
        CreateMap<Breed, BreedDto>().ReverseMap();
    }
}
