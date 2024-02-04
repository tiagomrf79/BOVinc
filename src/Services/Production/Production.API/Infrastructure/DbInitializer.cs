using CsvHelper;
using CsvHelper.Configuration;
using Production.API.Mappings;
using Production.API.Models;
using System.Globalization;

namespace Production.API.Infrastructure;

public static class DbInitializer
{
    public static void ImportFactors(IApplicationBuilder applicationBuilder)
    {
        ProductionContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ProductionContext>();

        if (!context.FirstTestFactors.Any())
        {
            IEnumerable<FirstTestFactor> firstTestFactors;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "FactorsForAdjustingSampleDayYieldForFirstTestInterval.csv");

            var configuration = new CsvConfiguration(new CultureInfo("pt-PT")) { Delimiter = ";" };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                csv.Context.RegisterClassMap<FirstTestFactorMap>();
                firstTestFactors = csv.GetRecords<FirstTestFactor>();
            }

            context.FirstTestFactors.AddRange(firstTestFactors);
        }

        if (!context.PeakTestFactors.Any())
        {
            IEnumerable<PeakTestFactor> peakTestFactors;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "FactorsForAdjustingYieldsForTestIntervalAfterLastSampleDay..csv");

            var configuration = new CsvConfiguration(new CultureInfo("pt-PT")) { Delimiter = ";" };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                csv.Context.RegisterClassMap<PeakTestFactorMap>();
                peakTestFactors = csv.GetRecords<PeakTestFactor>();
            }

            context.PeakTestFactors.AddRange(peakTestFactors);
        }

        if (!context.LastTestFactors.Any())
        {
            IEnumerable<LastTestFactor> lastTestFactors;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "FactorsForAdjustingYieldsForTestIntervalAfterLastSampleDay..csv");

            var configuration = new CsvConfiguration(new CultureInfo("pt-PT")) { Delimiter = ";" };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, configuration))
            {
                csv.Context.RegisterClassMap<LastTestFactorMap>();
                lastTestFactors = csv.GetRecords<LastTestFactor>();
            }

            context.LastTestFactors.AddRange(lastTestFactors);
        }
    }
}
