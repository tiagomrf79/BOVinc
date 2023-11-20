using CsvHelper.Configuration.Attributes;

namespace AnimalsAPI.Models.csv;

public class AnimalRecord
{
    [Name("Id")] public int? Id { get; set; }
    [Name("FarmId")] public int? FarmId { get; set; }

    [Name("RegistrationId")] public string? RegistrationId { get; set; }
    [Name("Name")] public string? Name { get; set; }
    [Name("Tag")] public string? Tag { get; set; }
    [Name("Gender")] public int? GenderIndex { get; set; }
    [Name("Breed")] public int? BreedIndex { get; set; }
    [Name("DateOfBirth")] public DateOnly? DateOfBirth { get; set; }
    [Name("MotherId")] public int? MotherId { get; set; }
    [Name("FatherId")] public int? FatherId { get; set; }
}
