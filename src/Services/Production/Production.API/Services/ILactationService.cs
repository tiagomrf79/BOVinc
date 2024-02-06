using Production.API.DTOs;

namespace Production.API.Services;

public interface ILactationService
{
    Task<List<YieldRecordDto>> GetAdjustedMilkYields(List<YieldRecordDto> records, int lactationNumber);
    Task<int> GetFatTotalYieldAsync(List<YieldRecordDto> records, int lactationNumber);
    Task<int> GetProteinTotalYieldAsync(List<YieldRecordDto> records, int lactationNumber);
    Task<int> GetMilkTotalYieldStandardizedAsync(List<YieldRecordDto> records, int lactationNumber);
    Task<int> GetFatTotalYieldStandardizedAsync(List<YieldRecordDto> records, int lactationNumber);
    Task<int> GetProteinTotalYieldStandardizedAsync(List<YieldRecordDto> records, int lactationNumber);
    Task<List<Tuple<DateOnly, double, bool>>> GetLactationCurve(List<YieldRecordDto> records, int lactationNumber);
}
