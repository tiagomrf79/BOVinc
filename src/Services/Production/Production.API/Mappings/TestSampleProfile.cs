using AutoMapper;
using Production.API.DTOs;
using Production.API.Models;

namespace Production.API.Mappings;

public class TestSampleProfile : Profile
{
    public TestSampleProfile()
    {
        CreateMap<TestSample, FullSampleDto>().ReverseMap();
    }
}
