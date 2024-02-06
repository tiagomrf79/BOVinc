using Production.API.DTOs;

namespace Production.API.Services;

public interface ILactationService
{
    Task<List<IntervalYieldDto>> GetAdjustedMilkYields(List<YieldRecordDto> records, int lactationNumber);
    Task<List<IntervalYieldDto>> GetAdjustedFatYields(List<YieldRecordDto> records, int lactationNumber);
    Task<List<IntervalYieldDto>> GetAdjustedProteinYields(List<YieldRecordDto> records, int lactationNumber);
    Task<List<Tuple<DateOnly, double, bool>>> GetLactationCurve(List<YieldRecordDto> records, int lactationNumber);
}
