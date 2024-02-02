using CsvHelper.Configuration;
using Production.API.Models;

namespace Production.API.Mappings;

public class FirstTestFactorMap : ClassMap<FirstTestFactor>
{
    public FirstTestFactorMap()
    {
        Map(x => x.DayOfFirstSampleMin).Index(0);

        Map(x => x.DayOfFirstSampleMax).Index(1);

        Map(x => x.IsFirstlactation).Index(2);

        Map(x => x.MilkFactor).Index(3);

        Map(x => x.FatFactor).Index(4);

        Map(x => x.ProteinFactor).Index(5);
    }
}
