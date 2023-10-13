using CsvHelper.Configuration.Attributes;

namespace FarmsAPI.Models.csv;

public class FarmRecord
{
    [Name("Id")] public int? Id { get; set; }
    [Name("Name")] public string? Name { get; set; }
    [Name("Address")] public string? Address { get; set; }
    [Name("City")] public string? City { get; set; }
    [Name("Region")] public string? Region { get; set; }
    [Name("Country")] public string? Country { get; set; }
}
