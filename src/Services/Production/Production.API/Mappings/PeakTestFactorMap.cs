using CsvHelper.Configuration;
using Production.API.Models;

namespace Production.API.Mappings;

public class PeakTestFactorMap : ClassMap<PeakTestFactor>
{
    public PeakTestFactorMap()
    {
        Map(x => x.DayOfFirstSampleMin).Index(0);

        Map(x => x.DayOfFirstSampleMax).Index(1);

        Map(x => x.TestIntervalMin).Index(2);

        Map(x => x.TestIntervalMax).Index(3);

        Map(x => x.IsFirstlactation).Index(4);

        Map(x => x.MilkFactor).Index(5);

        Map(x => x.FatFactor).Index(6);

        Map(x => x.ProteinFactor).Index(7);
    }
}
