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

    public Task<double> GetFactor<TType, TTrait>(
        TType type, TTrait trait,
        int daysInMilk, bool isFirstlactation, int? daysInTestInterval = null)
            where TType : FactorType
            where TTrait : YieldType
    {

        var parameters = new MyParameters(isFirstlactation, daysInMilk, daysInTestInterval);

        var query = type.GetQuery(_context, parameters);

        var result = trait.GetResult(query);

        return result;
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

    private IQueryable<FirstTestFactor> GetFactorsForFirstTestInterval(int daysInMilk, bool isFirstLactation)
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

    private async Task<double> GetFactorAsync(Func<FactorType, double> factorSelector, IQueryable<FactorType> query)
    {
        return await query.Select(factorSelector).FirstOrDefaultAsync();
    }

    public async Task<double> GetMilkFactorForFirstTestIntervalAsync2(int daysInMilk, bool isFirstLactation)
    {
        var query = GetFactorsForFirstTestIntervalQuery(daysInMilk, isFirstLactation);
        return await GetFactorAsync(x => x.MilkFactor, query);
    }

    private IQueryable<FirstTestFactor> GetFactorsForFirstTestIntervalQuery(int daysInMilk, bool isFirstLactation)
    {
        return _context.FirstTestFactors.Where(x =>
            x.IsFirstlactation == isFirstLactation &&
            daysInMilk >= x.DayOfFirstSampleMin &&
            daysInMilk <= x.DayOfFirstSampleMax);
    }

    /*public interface IYield<T> where T : FactorType
    {
        Task<double> GetResult(IQueryable<T> query);
    }

    public class MilkFactor<T> : IYield<T> where T : FactorType
    {
        public async Task<double> GetResult(IQueryable<T> query)
        {
            return await query.Select(x => x.MilkFactor).FirstOrDefaultAsync();
        }
    }

    public class FatFactor<T> : IYield<T> where T : FactorType
    {
        public async Task<double> GetResult(IQueryable<T> query)
        {
            return await query.Select(x => x.FatFactor).FirstOrDefaultAsync();
        }
    }*/

    public abstract class YieldType
    {
        public abstract Task<double> GetResult<T>(IQueryable<T> query) where T : FactorType;
    }

    public class MilkType : YieldType
    {
        public override async Task<double> GetResult<T>(IQueryable<T> query)
        {
            return await query.Select(x => x.MilkFactor).FirstOrDefaultAsync();
        }
    }
    public class FatType : YieldType
    {
        public override async Task<double> GetResult<T>(IQueryable<T> query)
        {
            return await query.Select(x => x.FatFactor).FirstOrDefaultAsync();
        }
    }


    public abstract class FactorType
    {
        public bool IsFirstLactation { get; set; }
        public double MilkFactor { get; set; }
        public double FatFactor { get; set; }

        public abstract IQueryable<T> GetQuery<T>(ProductionContext context, MyParameters parameters) where T : FactorType;
    }

    public class FirstFactor : FactorType
    {
        public int DayOfFirstSampleMin { get; set; }
        public int DayOfFirstSampleMax { get; set; }

        public override IQueryable<FirstTestFactor> GetQuery<FirstTestFactor>(ProductionContext context, MyParameters parameters)
        {
            return (IQueryable<FirstTestFactor>)context.FirstTestFactors.Where(x =>
                x.IsFirstlactation == parameters.IsFirstLactation
                && parameters.DaysInMilk >= x.DayOfFirstSampleMin
                && parameters.DaysInMilk <= x.DayOfFirstSampleMax);
        }
    }

    public class PeakFactor : FactorType
    {
        public int DayOfPreviousSampleMin { get; set; }
        public int DayOfPreviousSampleMax { get; set; }
        public int TestIntervalMin { get; set; }
        public int TestIntervalMax { get; set; }

        public override IQueryable<PeakTestFactor> GetQuery<PeakTestFactor>(ProductionContext context, MyParameters parameters)
        {
            return (IQueryable<PeakTestFactor>)context.PeakTestFactors
                .Where(x =>
                    x.IsFirstlactation == parameters.IsFirstLactation
                    && parameters.DaysInMilk >= x.DayOfPreviousSampleMin
                    && parameters.DaysInMilk <= x.DayOfPreviousSampleMax
                    && parameters.DaysInTestInterval >= x.TestIntervalMin
                    && parameters.DaysInTestInterval <= x.TestIntervalMax);
        }
    }

    public record MyParameters (
        bool IsFirstLactation,
        int DaysInMilk,
        int? DaysInTestInterval
    );


}
