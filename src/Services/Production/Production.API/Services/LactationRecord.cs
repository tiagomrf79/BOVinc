using Microsoft.EntityFrameworkCore;
using Production.API.Infrastructure;

namespace Production.API.Services;

public class LactationRecord
{
    private readonly ProductionContext _productionContext;

    public LactationRecord(ProductionContext productionContext)
    {
        _productionContext = productionContext;
    }

    private async Task<List<IntervalYield>> CalculateRecordedYield(YieldTrait yieldTrait, List<YieldRecord> yields, int lactationNumber)
    {
        yields = yields
            .Where(x => x.DaysInMilk >= 6) // ignore test-day samples on the first 5 days after calving
            .OrderBy(x => x.DaysInMilk)
            .ToList();

        bool isFirstLactation = lactationNumber == 1;

        List<IntervalYield> intervalYields = new();

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

            int start, end;
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

            start = previousItemDim;
            end = currentItemDim;

            IntervalYield currentInterval = new(start, end, yield);
            intervalYields.Add(currentInterval);
        }

        return intervalYields;
    }

    private async Task<List<IntervalYield>> AdjustYieldTo305Days(YieldTrait yieldTrait, List<YieldRecord> yields, int lactationNumber)
    {
        yields = yields
            .Where(x => x.DaysInMilk >= 6) // ignore test-day samples on the first 5 days after calving
            .OrderBy(x => x.DaysInMilk)
            .ToList();

        bool isFirstLactation = lactationNumber == 1;

        List<IntervalYield> intervalYields = new();

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

            int start, end;
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

            start = previousItemDim;
            end = currentItemDim > 305 ? 305 : currentItemDim;

            IntervalYield currentInterval = new(start, end, yield);
            intervalYields.Add(currentInterval);

            // stop iterating on the first test-day sample at or after 305 days
            if (currentItemDim >= 305)
                break;

            // predict remaining yield when the lactation is shorter than 305 days
            if (lastItem && currentItemDim < 305)
            {
                double factor = await GetFactorForTestIntervalAfterLastSampleDay(
                    yieldTrait, currentItemDim, 305 - currentItemDim, isFirstLactation);
                yield = factor * currentItemYield;
                IntervalYield lastInterval = new(currentItemDim, 305, yield);
                intervalYields.Add(lastInterval);
            }
        }

        return intervalYields;
    }

    private async Task<double> GetFactorForTestIntervalAtPeakOfLactation(
        YieldTrait yieldTrait, int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        var row = _productionContext.PeakTestFactors
            .Where(x => 
                x.IsFirstlactation == isFirstLactation
                && daysInMilk >= x.DayOfPreviousSampleMin
                && daysInMilk <= x.DayOfPreviousSampleMax
                && daysInTestInterval >= x.TestIntervalMin
                && daysInTestInterval <= x.TestIntervalMax);

        if (yieldTrait == YieldTrait.Milk)
            return await row.Select(x => x.MilkFactor).FirstOrDefaultAsync();
        else if (yieldTrait == YieldTrait.Fat)
            return await row.Select(x => x.FatFactor).FirstOrDefaultAsync();
        else if (yieldTrait == YieldTrait.Protein)
            return await row.Select(x => x.ProteinFactor).FirstOrDefaultAsync();

        return 1;
    }

    private async Task<double> GetFactorForFirstTestInterval(YieldTrait yieldTrait, int daysInMilk, bool isFirstLactation)
    {
        var row = _productionContext.FirstTestFactors
            .Where(x => 
                x.IsFirstlactation == isFirstLactation
                && daysInMilk >= x.DayOfFirstSampleMin
                && daysInMilk <= x.DayOfFirstSampleMax);

        if (yieldTrait == YieldTrait.Milk)
            return await row.Select(x => x.MilkFactor).FirstOrDefaultAsync();
        else if (yieldTrait == YieldTrait.Fat)
            return await row.Select(x => x.FatFactor).FirstOrDefaultAsync();
        else if (yieldTrait == YieldTrait.Protein)
            return await row.Select(x => x.ProteinFactor).FirstOrDefaultAsync();

        return 1;
    }

    private async Task<double> GetFactorForTestIntervalAfterLastSampleDay(
        YieldTrait yieldTrait, int daysInMilk, int daysInTestInterval, bool isFirstLactation)
    {
        var row = _productionContext.LastTestFactors
            .Where(x =>
                x.IsFirstlactation == isFirstLactation
                && daysInMilk >= x.DayOfLastSampleMin
                && daysInMilk <= x.DayOfLastSampleMax
                && daysInTestInterval >= x.TestIntervalMin
                && daysInTestInterval <= x.TestIntervalMax);

        if (yieldTrait == YieldTrait.Milk)
            return await row.Select(x => x.MilkFactor).FirstOrDefaultAsync();
        else if (yieldTrait == YieldTrait.Fat)
            return await row.Select(x => x.FatFactor).FirstOrDefaultAsync();
        else if (yieldTrait == YieldTrait.Protein)
            return await row.Select(x => x.ProteinFactor).FirstOrDefaultAsync();

        return 1;
    }

    private record YieldRecord (int DaysInMilk, double Yield);
    private record IntervalYield (int Start, int End, double Yield);
    private enum YieldTrait
    {
        Milk = 1,
        Fat = 2,
        Protein = 3
    }
}

