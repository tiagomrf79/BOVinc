namespace Production.API.Models;

public class FirstTestFactor
{
    public int DayOfFirstSampleMin { get; set; }
    public int DayOfFirstSampleMax { get; set; }

    public bool IsFirstlactation { get; set; }

    public double MilkFactor { get; set; }
    public double FatFactor { get; set; }
    public double ProteinFactor { get; set; }
}
