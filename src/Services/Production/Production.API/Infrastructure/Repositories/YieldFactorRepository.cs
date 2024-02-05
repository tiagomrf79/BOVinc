
using Microsoft.EntityFrameworkCore;
using Production.API.Models;

namespace Production.API.Infrastructure.Repositories;

public class YieldFactorRepository : IYieldFactorRepository
{
    private readonly ProductionContext _context;

    public YieldFactorRepository(ProductionContext context)
    {
        _context = context;
    }

    public async Task<double> GetMilkFactorForFirstTestIntervalAsync(int daysInMilk, bool isFirstLactation)
    {
        return await GetFactorsForFirstTestInterval(daysInMilk, isFirstLactation)
            .Select(x => x.MilkFactor)
            .FirstOrDefaultAsync();
    }

    public async Task<double> GetFatFactorForFirstTestIntervalAsync(int daysInMilk, bool isFirstLactation)
    {
        return await GetFactorsForFirstTestInterval(daysInMilk, isFirstLactation)
            .Select(x => x.FatFactor)
            .FirstOrDefaultAsync();
    }

    public async Task<double> GetProteinFactorForFirstTestIntervalAsync(int daysInMilk, bool isFirstLactation)
    {
        return await GetFactorsForFirstTestInterval(daysInMilk, isFirstLactation)
            .Select(x => x.ProteinFactor)
            .FirstOrDefaultAsync();
    }

    public async Task<double> GetMilkFactorForTestIntervalAtPeakOfLactationAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        return await GetFactorsForTestIntervalAtPeakOfLactation(daysInMilk, daysInTestInterval, isFirstLactation)
            .Select(x => x.MilkFactor)
            .FirstOrDefaultAsync();
    }

    public async Task<double> GetFatFactorForTestIntervalAtPeakOfLactationAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        return await GetFactorsForTestIntervalAtPeakOfLactation(daysInMilk, daysInTestInterval, isFirstLactation)
            .Select(x => x.FatFactor)
            .FirstOrDefaultAsync();
    }

    public async Task<double> GetProteinFactorForTestIntervalAtPeakOfLactationAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        return await GetFactorsForTestIntervalAtPeakOfLactation(daysInMilk, daysInTestInterval, isFirstLactation)
            .Select(x => x.ProteinFactor)
            .FirstOrDefaultAsync();
    }

    public async Task<double> GetMilkFactorForTestIntervalAfterLastSampleDayAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        return await GetFactorsForTestIntervalAfterLastSampleDay(daysInMilk, daysInTestInterval, isFirstLactation)
            .Select(x => x.MilkFactor)
            .FirstOrDefaultAsync();
    }

    public async Task<double> GetFatFactorForTestIntervalAfterLastSampleDayAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        return await GetFactorsForTestIntervalAfterLastSampleDay(daysInMilk, daysInTestInterval, isFirstLactation)
            .Select(x => x.FatFactor)
            .FirstOrDefaultAsync();
    }

    public async Task<double> GetProteinFactorForTestIntervalAfterLastSampleDayAsync(int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        return await GetFactorsForTestIntervalAfterLastSampleDay(daysInMilk, daysInTestInterval, isFirstLactation)
            .Select(x => x.ProteinFactor)
            .FirstOrDefaultAsync();
    }

    private IQueryable<YieldFactors> GetFactorsForFirstTestInterval(int daysInMilk, bool isFirstLactation)
    {
        return _context.FirstTestFactors
            .Where(x => 
                x.IsFirstlactation == isFirstLactation
                && daysInMilk >= x.DayOfFirstSampleMin
                && daysInMilk <= x.DayOfFirstSampleMax);
    }

    private IQueryable<PeakTestFactor> GetFactorsForTestIntervalAtPeakOfLactation(int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        return _context.PeakTestFactors
            .Where(x =>
                x.IsFirstlactation == isFirstLactation
                && daysInMilk >= x.DayOfPreviousSampleMin
                && daysInMilk <= x.DayOfPreviousSampleMax
                && daysInTestInterval >= x.TestIntervalMin
                && daysInTestInterval <= x.TestIntervalMax);
    }

    private IQueryable<LastTestFactor> GetFactorsForTestIntervalAfterLastSampleDay(int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        return _context.LastTestFactors
            .Where(x =>
                x.IsFirstlactation == isFirstLactation
                && daysInMilk >= x.DayOfLastSampleMin
                && daysInMilk <= x.DayOfLastSampleMax
                && daysInTestInterval >= x.TestIntervalMin
                && daysInTestInterval <= x.TestIntervalMax);
    }
}
