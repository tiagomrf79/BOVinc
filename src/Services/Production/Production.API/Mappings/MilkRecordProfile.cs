using AutoMapper;
using Production.API.DTOs;
using Production.API.Models;

namespace Production.API.Mappings;

public class MilkRecordProfile : Profile
{
    public MilkRecordProfile()
    {
        CreateMap<MilkRecord, MilkRecordDto>().ReverseMap();
    }
}
