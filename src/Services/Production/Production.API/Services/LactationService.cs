using Production.API.DTOs;
using Production.API.Infrastructure.Repositories;

namespace Production.API.Services;

public class LactationService : ILactationService
{
    private readonly IYieldFactorRepository _yieldFactorRepository;

    public LactationService(IYieldFactorRepository yieldFactorRepository)
    {
        _yieldFactorRepository = yieldFactorRepository;
    }

    public async Task<List<IntervalYieldDto>> GetAdjustedMilkYields(List<YieldRecordDto> records, int lactationNumber)
    {
        bool isFirstLactation = lactationNumber == 1;
        return await GetAdjustedYields(YieldTrait.Milk , records, isFirstLactation);
    }

    public async Task<List<IntervalYieldDto>> GetAdjustedFatYields(List<YieldRecordDto> records, int lactationNumber)
    {
        bool isFirstLactation = lactationNumber == 1;
        return await GetAdjustedYields(YieldTrait.Fat, records, isFirstLactation);
    }

    public async Task<List<IntervalYieldDto>> GetAdjustedProteinYields(List<YieldRecordDto> records, int lactationNumber)
    {
        bool isFirstLactation = lactationNumber == 1;
        return await GetAdjustedYields(YieldTrait.Protein, records, isFirstLactation);
    }

    public async Task<List<IntervalYieldDto>> GetAdjustedMilkYield305Days(List<YieldRecordDto> records, int lactationNumber)
    {
        bool isFirstLactation = lactationNumber == 1;
        return await GetAdjustedYieldsFor305Days(YieldTrait.Milk, records, isFirstLactation);
    }

    public async Task<List<IntervalYieldDto>> GetAdjustedFatYield305Days(List<YieldRecordDto> records, int lactationNumber)
    {
        bool isFirstLactation = lactationNumber == 1;
        return await GetAdjustedYieldsFor305Days(YieldTrait.Fat, records, isFirstLactation);
    }

    public async Task<List<IntervalYieldDto>> GetAdjustedProteinYield305Days(List<YieldRecordDto> records, int lactationNumber)
    {
        bool isFirstLactation = lactationNumber == 1;
        return await GetAdjustedYieldsFor305Days(YieldTrait.Protein, records, isFirstLactation);
    }

    //public async Task<TotalDto> GetTotalYield(
    //    List<double> milkYields,
    //    List<double?> fatYields,
    //    List<double?> proteinYields,
    //    List<int> daysInMilk,
    //    int lactationNumber)
    //{
    //    // milkYields and daysInMilk should have the same number of items
    //    if (daysInMilk.Count != milkYields.Count)
    //        return Tuple.Create(0, 0, 0);

    //    bool isFirstLactation = lactationNumber == 1;

    //    for (int i = 0; i < daysInMilk.Count; i++)
    //    {
    //        if (fatYields[i] != null)
    //        {

    //        }
    //        if (proteinYields[i] != null)
    //        {

    //        }
    //    }

    //    int milkTotal = await GetTotalYieldForTrait(YieldTrait.Milk, daysInMilk, milkYields, isFirstLactation);
    //    int fatTotal = await GetTotalYieldForTrait(YieldTrait.Fat, daysInMilk, fatYields, isFirstLactation);
    //    int proteinTotal = await GetTotalYieldForTrait(YieldTrait.Protein, daysInMilk, proteinYields, isFirstLactation);

    //    return Tuple.Create(milkTotal, fatTotal, proteinTotal);
    //}

    //public async Task<Tuple<int, int, int>> GetTotalYieldStandardized(
    //    List<double> milkYields,
    //    List<double?> fatYields,
    //    List<double?> proteinYields,
    //    List<int> daysInMilk,
    //    int lactationNumber)
    //{
    //    // milkYields and daysInMilk should have the same number of items
    //    if (daysInMilk.Count != milkYields.Count)
    //        return Tuple.Create(0, 0, 0);

    //    bool isFirstLactation = lactationNumber == 1;

    //    int milkTotal = await Get305DaysYieldForTrait(YieldTrait.Milk, daysInMilk, milkYields, isFirstLactation);
    //    int fatTotal = await Get305DaysYieldForTrait(YieldTrait.Fat, daysInMilk, fatYields, isFirstLactation);
    //    int proteinTotal = await Get305DaysYieldForTrait(YieldTrait.Protein, daysInMilk, proteinYields, isFirstLactation);

    //    return Tuple.Create(milkTotal, fatTotal, proteinTotal);
    //}

    public async Task<List<Tuple<DateOnly, double, bool>>> GetLactationCurve(List<YieldRecordDto> records, int lactationNumber)
    {
        throw new NotImplementedException();
    }

    //private async Task<int> GetTotalYieldForTrait(YieldTrait trait, List<int> days, List<double> yields, bool isFirstLactation)
    //{
    //    List<YieldRecord> records = days.Zip(yields, (x, y) => new YieldRecord(x, y)).ToList();
    //    List<IntervalYield> intervals = await GetIntervalYields(trait, records, isFirstLactation);
    //    double total = intervals.Sum(x => (x.End - x.Start) * x.Yield);

    //    return (int)Math.Round(total, MidpointRounding.AwayFromZero);
    //}

    //private async Task<int> Get305DaysYieldForTrait(YieldTrait trait, List<int> days, List<double> yields, bool isFirstLactation)
    //{
    //    List<YieldRecord> records = days.Zip(yields, (x, y) => new YieldRecord(x, y)).ToList();
    //    List<IntervalYield> intervals = await GetIntervalYieldsFor305Days(trait, records, isFirstLactation);
    //    double total = intervals.Sum(x => (x.End - x.Start) * x.Yield);

    //    return (int)Math.Round(total, MidpointRounding.AwayFromZero);
    //}

    private async Task<List<IntervalYieldDto>> GetAdjustedYields(YieldTrait yieldTrait, List<YieldRecordDto> yields, bool isFirstLactation)
    {
        yields = yields
            .Where(x => x.DaysInMilk >= 6) // ignore test-day samples on the first 5 days after calving
            .OrderBy(x => x.DaysInMilk)
            .ToList();

        List<IntervalYieldDto> adjustedYields = new();

        for (int i = 0; i < yields.Count; i++)
        {
            bool firstItem = i == 0;
            bool lastItem = i == yields.Count - 1;

            int currentItemDim = yields[i].DaysInMilk;
            int previousItemDim = firstItem ? 0 : yields[i - 1].DaysInMilk;

            // ignore any records that occur on the same date (makes no sense to have duplicates)
            if (currentItemDim == previousItemDim)
                continue;

            double currentItemYield = yields[i].Yield;
            double previousItemYield = firstItem ? 0 : yields[i - 1].Yield;

            double yield;

            // estimate yield on the first interval
            if (firstItem)
            {
                double factor = await GetFactorForFirstTestInterval(yieldTrait, currentItemDim, isFirstLactation);
                yield = factor * currentItemYield;
            }
            // adjust yield when interval spans the peak of lactation
            else if (previousItemDim < 40 && currentItemDim >= 40)
            {
                int daysInTestInterval = currentItemDim - previousItemDim;
                double factor = await GetFactorForTestIntervalAtPeakOfLactation(
                    yieldTrait, currentItemDim, daysInTestInterval, isFirstLactation);
                yield = factor * (previousItemYield + currentItemYield) / 2;
            }
            else
            {
                yield = (previousItemYield + currentItemYield) / 2;
            }

            IntervalYieldDto currentInterval = new(previousItemDim, currentItemDim, yield);
            adjustedYields.Add(currentInterval);
        }

        return adjustedYields;
    }

    /// <summary>
    /// posso usar este método para calcular média de leite por dia, teor butiroso e teor proteico
    /// no caso da gordura a proteína vou ter de usar ainda outro método que multiplique o teor ajustado pela produção no intervalo
    /// </summary>
    private async Task<List<IntervalYieldDto>> GetAdjustedYieldsFor305Days(YieldTrait yieldTrait, List<YieldRecordDto> yields, bool isFirstLactation)
    {
        yields = yields
            .Where(x => x.DaysInMilk >= 6) // ignore test-day samples on the first 5 days after calving
            .OrderBy(x => x.DaysInMilk)
            .ToList();

        List<IntervalYieldDto> adjustedYields = new();

        for (int i = 0; i < yields.Count; i++)
        {
            bool firstItem = i == 0;
            bool lastItem = i == yields.Count - 1;

            int currentItemDim = yields[i].DaysInMilk;
            int previousItemDim = firstItem ? 0 : yields[i - 1].DaysInMilk;

            // ignore any records that occur on the same date (makes no sense to have duplicates)
            if (currentItemDim == previousItemDim)
                continue;

            double currentItemYield = yields[i].Yield;
            double previousItemYield = firstItem ? 0 : yields[i - 1].Yield;

            double yield;

            // estimate yield on the first interval
            if (firstItem)
            {
                double factor = await GetFactorForFirstTestInterval(yieldTrait, currentItemDim, isFirstLactation);
                yield = factor * currentItemYield;
            }
            // adjust yield when interval spans the peak of lactation
            else if (previousItemDim < 40 && currentItemDim >= 40)
            {
                int daysInTestInterval = currentItemDim - previousItemDim;
                double factor = await GetFactorForTestIntervalAtPeakOfLactation(
                    yieldTrait, currentItemDim, daysInTestInterval, isFirstLactation);
                yield = factor * (previousItemYield + currentItemYield) / 2;
            }
            // estimate yield at 305 days
            else if (currentItemDim > 305)
            {
                double dailyChange = (currentItemYield - previousItemYield) / (currentItemDim - previousItemDim);
                yield = previousItemYield + (305 - currentItemDim + 1) * dailyChange / 2;
                yield = yield < 0 ? 0 : yield;
            }
            else
            {
                yield = (previousItemYield + currentItemYield) / 2;
            }

            IntervalYieldDto currentInterval = new(previousItemDim, currentItemDim > 305 ? 305 : currentItemDim, yield);
            adjustedYields.Add(currentInterval);

            // stop iterating on the first test-day sample at or after 305 days
            if (currentItemDim >= 305)
                break;

            //TODO: this is not correct!
            // predict remaining yield when the lactation is shorter than 305 days
            if (lastItem && currentItemDim < 305)
            {
                double factor = await GetFactorForTestIntervalAfterLastSampleDay(
                    yieldTrait, currentItemDim, 305 - currentItemDim, isFirstLactation);

                yield = factor * currentItemYield;

                IntervalYieldDto lastInterval = new(currentItemDim, 305, yield);
                adjustedYields.Add(lastInterval);
            }
        }

        return adjustedYields;
    }

    private async Task<double> GetFactorForFirstTestInterval(YieldTrait yieldTrait, int daysInMilk, bool isFirstLactation)
    {
        if (yieldTrait == YieldTrait.Milk)
            return await _yieldFactorRepository.GetMilkFactorForFirstTestIntervalAsync(daysInMilk, isFirstLactation);
        else if (yieldTrait == YieldTrait.Fat)
            return await _yieldFactorRepository.GetFatFactorForFirstTestIntervalAsync(daysInMilk, isFirstLactation);
        else if (yieldTrait == YieldTrait.Protein)
            return await _yieldFactorRepository.GetProteinFactorForFirstTestIntervalAsync(daysInMilk, isFirstLactation);

        return 1;
    }

    private async Task<double> GetFactorForTestIntervalAtPeakOfLactation(
        YieldTrait yieldTrait, int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        if (yieldTrait == YieldTrait.Milk)
            return await _yieldFactorRepository.GetMilkFactorForTestIntervalAtPeakOfLactationAsync(
                daysInMilk, daysInTestInterval, isFirstLactation);
        else if (yieldTrait == YieldTrait.Fat)
            return await _yieldFactorRepository.GetFatFactorForTestIntervalAtPeakOfLactationAsync(
                daysInMilk, daysInTestInterval, isFirstLactation);
        else if (yieldTrait == YieldTrait.Protein)
            return await _yieldFactorRepository.GetProteinFactorForTestIntervalAtPeakOfLactationAsync(
                daysInMilk, daysInTestInterval, isFirstLactation);

        return 1;
    }

    private async Task<double> GetFactorForTestIntervalAfterLastSampleDay(
        YieldTrait yieldTrait, int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        var teste = new MilkFactorForFirstLactation();

        return teste.CalculateFactorForTestIntervalAfterLastSampleDay(daysInMilk, daysInTestInterval);
    }

    private enum YieldTrait
    {
        Milk = 1,
        Fat = 2,
        Protein = 3
    }

    
    private class TraitFactor
    {
        private readonly double _slope;
        private readonly double _intercept;

        protected TraitFactor(double slope, double intercept)
        {
            _slope = slope;
            _intercept = intercept;
        }

        public double CalculateFactorForFirstTestInterval(int daysInMilk)
        {
            return 0;
        }

        public double CalculateFactorForTestIntervalAtPeakOfLactation(int daysInMilk, int daysInTestInterval)
        {
            return 0;
        }

        public async Task<double> CalculateFactorForTestIntervalAfterLastSampleDay(int daysInMilk, int daysInTestInterval)
        {
            return await _yieldFactorRepository.GetMilkFactorForTestIntervalAtPeakOfLactationAsync(
                daysInMilk, daysInTestInterval, isFirstLactation);

            //return 1 - 0.5 * _slope * daysInTestInterval / (_intercept - _slope * daysInMilk);
        }
    }

    private class MilkFactorForFirstLactation : TraitFactor
    {
        const double intercept = 48.3;
        const double slope = 0.071;
        public MilkFactorForFirstLactation() : base(intercept, slope) { }
    }

    private class MilkFactorForOtherLactation : TraitFactor
    {
        const double intercept = 71;
        const double slope = 0.144;
        public MilkFactorForOtherLactation() : base(intercept, slope) { }
    }

    private class FatFactorForFirstLactation : TraitFactor
    {
        const double intercept = 2.03;
        const double slope = 0.0025;
        public FatFactorForFirstLactation() : base(intercept, slope) { }
    }

    private class FatFactorForOtherLactation : TraitFactor
    {
        const double intercept = 2.78;
        const double slope = 0.0052;
        public FatFactorForOtherLactation() : base(intercept, slope) { }
    }
}

