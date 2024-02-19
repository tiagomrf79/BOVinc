using Animal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Animal.API.Infrastructure;

public class AnimalContextSeed
{
    public static async Task SeedAsync(AnimalContext animalContext)
    {
        if (!await animalContext.Breeds.AnyAsync())
        {
            await animalContext.Breeds.AddRangeAsync(
                new Breed()
                {
                    Id = 1,
                    Name = "Holstein-Frisia",
                    GestationLength = 279,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new Breed()
                {
                    Id = 2,
                    Name = "Jersey",
                    GestationLength = 283,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new Breed()
                {
                    Id = 3,
                    Name = "Brown Swiss",
                    GestationLength = 291,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                });

            animalContext.Database.BeginTransaction();
            animalContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Breed ON;");
            await animalContext.SaveChangesAsync();
            animalContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Breed OFF;");
            animalContext.Database.CommitTransaction();
        }

        if (!await animalContext.FarmAnimals.AnyAsync())
        {
            await animalContext.FarmAnimals.AddRangeAsync(
                new FarmAnimal()
                {
                    Id = 59,
                    Name = "Xepa",
                    SexId = 1,
                    BreedId = 1,
                    IsActive = false,
                    CatalogId = 2
                },
                new FarmAnimal()
                {
                    Id = 97,
                    Name = "Mimosa",
                    SexId = 1,
                    BreedId = 1,
                    IsActive = false,
                    CatalogId = 2
                },
                new FarmAnimal()
                {
                    Id = 1,
                    RegistrationId = "PT 219 144848",
                    Name = "Marquesa",
                    DateOfBirth = new DateOnly(2016,8,28),
                    DamId = 59,
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 3,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 1,
                    Notes = "Best cow ever",
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 2,
                    RegistrationId = "PT 266 483932",
                    Name = "",
                    DateOfBirth = new DateOnly(2012, 7, 29),
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 2,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 3,
                    RegistrationId = "PT 829 999162",
                    Name = "Calçada",
                    DateOfBirth = new DateOnly(2006, 7, 27),
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 3,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 4,
                    RegistrationId = "PT 337 386951",
                    Name = "Tieta",
                    DateOfBirth = new DateOnly(2011, 12, 1),
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 3,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 5,
                    RegistrationId = "PT 454 199443",
                    Name = "Xepa",
                    DateOfBirth = new DateOnly(1997, 4, 11),
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 3,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 6,
                    RegistrationId = "PT 536 445260",
                    Name = "Malhinha",
                    DateOfBirth = new DateOnly(1994, 1, 27),
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 3,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 7,
                    RegistrationId = "PT 437 619249",
                    Name = "Mimosa",
                    DateOfBirth = new DateOnly(2016, 7, 1),
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 3,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 8,
                    RegistrationId = "PT 404 653293",
                    Name = "Francisca",
                    DateOfBirth = new DateOnly(2007, 4, 27),
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 4,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 9,
                    RegistrationId = "PT 394 975382",
                    Name = "",
                    DateOfBirth = new DateOnly(2022, 8, 8),
                    SexId = 1,
                    BreedId = 2,
                    CategoryId = 2,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 3,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 10,
                    RegistrationId = "PT 295 977381",
                    Name = "",
                    DateOfBirth = new DateOnly(2023, 3, 16),
                    DamId = 1,
                    SireId = 13,
                    SexId = 2,
                    BreedId = 1,
                    CategoryId = 1,
                    PurposeId = 5,
                    IsActive = true,
                    CatalogId = 3,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 11,
                    RegistrationId = "PT 955 960691",
                    Name = "",
                    DateOfBirth = new DateOnly(2023, 5, 14),
                    SireId = 13,
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 1,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 3,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 12,
                    RegistrationId = "PT 476 545412",
                    Name = "",
                    DateOfBirth = new DateOnly(2021, 12, 31),
                    SexId = 1,
                    BreedId = 1,
                    CategoryId = 2,
                    PurposeId = 2,
                    IsActive = true,
                    CatalogId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new FarmAnimal()
                {
                    Id = 13,
                    RegistrationId = "PT 119 264935",
                    Name = "Malhão",
                    DateOfBirth = new DateOnly(2021, 8, 5),
                    SexId = 2,
                    BreedId = 1,
                    CategoryId = 5,
                    PurposeId = 1,
                    IsActive = true,
                    CatalogId = 3,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                });

            animalContext.Database.BeginTransaction();
            animalContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Animal ON;");
            await animalContext.SaveChangesAsync();
            animalContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Animal OFF;");
            animalContext.Database.CommitTransaction();
        }

        if (!await animalContext.AnimalStatus.AnyAsync())
        {
            await animalContext.AnimalStatus.AddRangeAsync(
                new AnimalStatus()
                {
                    AnimalId = 1,
                    CurrentGroupName = "Main barn",
                    LastCalvingDate = new DateOnly(2023, 12, 26),
                    MilkingStatusId = 1,
                    BreedingStatusId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new AnimalStatus()
                {
                    AnimalId = 2,
                    CurrentGroupName = "Maternity pen",
                    MilkingStatusId = 2,
                    LastBreedingBull = "Malhão",
                    LastDryDate = new DateOnly(2023, 11, 11),
                    DueDateForCalving = new DateOnly(2024, 1, 11),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new AnimalStatus()
                {
                    AnimalId = 3,
                    LastCalvingDate = new DateOnly(2023, 3, 17),
                    MilkingStatusId = 1,
                    BreedingStatusId = 3,
                    LastBreedingDate = new DateOnly(2023, 5, 31),
                    DueDateForCalving = new DateOnly(2024, 3, 14),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow

                },
                new AnimalStatus()
                {
                    AnimalId = 4,
                    LastCalvingDate = new DateOnly(2023, 12, 21),
                    MilkingStatusId = 1,
                    BreedingStatusId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new AnimalStatus()
                {
                    AnimalId = 5,
                    LastCalvingDate = new DateOnly(2023, 12, 26),
                    MilkingStatusId = 1,
                    BreedingStatusId = 2,
                    LastBreedingDate = new DateOnly(2023, 12, 17),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new AnimalStatus()
                {
                    AnimalId = 6,
                    LastCalvingDate = new DateOnly(2023, 4, 1),
                    MilkingStatusId = 1,
                    BreedingStatusId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new AnimalStatus()
                {
                    AnimalId = 7,
                    LastCalvingDate = new DateOnly(2023, 4, 1),
                    MilkingStatusId = 1,
                    BreedingStatusId = 3,
                    LastBreedingDate = new DateOnly(2023, 7, 27),
                    DueDateForCalving = new DateOnly(2024, 5, 9),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new AnimalStatus()
                {
                    AnimalId = 8,
                    MilkingStatusId = 2,
                    LastDryDate = new DateOnly(2023, 11, 11),
                    DueDateForCalving = new DateOnly(2024, 1, 23),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new AnimalStatus()
                {
                    AnimalId = 9,
                    BreedingStatusId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new AnimalStatus()
                {
                    AnimalId = 12,
                    CurrentGroupName = "Late heifers",
                    BreedingStatusId = 3,
                    LastBreedingDate = new DateOnly(2023, 6, 19),
                    DueDateForCalving = new DateOnly(2024, 3, 8),
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                });

            await animalContext.SaveChangesAsync();
        }

        if (!await animalContext.Lactations.AnyAsync())
        {
            await animalContext.Lactations.AddRangeAsync(
                new Lactation()
                {
                    Id = 1,
                    LactationNumber = 3,
                    CalvingDate = new DateOnly(2023,12,26),
                    FarmAnimalId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new Lactation()
                {
                    Id = 2,
                    LactationNumber = 2,
                    CalvingDate = new DateOnly(2022, 12, 13),
                    EndDate = new DateOnly(2023,10,24),
                    FarmAnimalId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new Lactation()
                {
                    Id = 3,
                    LactationNumber = 1,
                    CalvingDate = new DateOnly(2021, 12, 8),
                    EndDate = new DateOnly(2022, 10, 13),
                    FarmAnimalId = 1,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new Lactation()
                {
                    Id = 4,
                    LactationNumber = 5,
                    CalvingDate = new DateOnly(2023, 1, 20),
                    EndDate = new DateOnly(2023, 11, 11),
                    FarmAnimalId = 2,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                },
                new Lactation()
                {
                    Id = 5,
                    LactationNumber = 4,
                    CalvingDate = new DateOnly(2021, 12, 17),
                    EndDate = new DateOnly(2022, 11, 24),
                    FarmAnimalId = 2,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                });

            animalContext.Database.BeginTransaction();
            animalContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Lactation ON;");
            await animalContext.SaveChangesAsync();
            animalContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Lactation OFF;");
            animalContext.Database.CommitTransaction();
        }
    }
}
