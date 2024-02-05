namespace Production.API.Infrastructure.Repositories;

public interface IYieldFactorRepository
{
    Task<double> GetMilkFactorForFirstTestIntervalAsync(int daysInMilk, bool isFirstLactation);
    Task<double> GetFatFactorForFirstTestIntervalAsync(int daysInMilk, bool isFirstLactation);
    Task<double> GetProteinFactorForFirstTestIntervalAsync(int daysInMilk, bool isFirstLactation);

    Task<double> GetMilkFactorForTestIntervalAtPeakOfLactationAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation);
    Task<double> GetFatFactorForTestIntervalAtPeakOfLactationAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation);
    Task<double> GetProteinFactorForTestIntervalAtPeakOfLactationAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation);

    Task<double> GetMilkFactorForTestIntervalAfterLastSampleDayAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation);
    Task<double> GetFatFactorForTestIntervalAfterLastSampleDayAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation);
    Task<double> GetProteinFactorForTestIntervalAfterLastSampleDayAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation);
}
