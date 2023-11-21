using AnimalsAPI.DbContexts;
using AutoMapper;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using AnimalsAPI.Models.csv;
using AnimalsAPI.Models;

namespace AnimalsAPI.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly AnimalsDbContext _context;
    private readonly ILogger<SeedController> _logger;
    private IWebHostEnvironment _env;
    private readonly IMapper _mapper;

    public SeedController(AnimalsDbContext context, ILogger<SeedController> logger, IWebHostEnvironment env, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _env = env;
        _mapper = mapper;
    }

    [HttpPut]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> SeedAnimalsData()
    {
        CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-PT"))
        {
            HasHeaderRecord = true,
            Delimiter = ";"
        };

        string fileFullPath = Path.Combine(_env.ContentRootPath, "Data/animals_dummy_dataset.csv");

        int addedRows = 0;
        int skippedRows = 0;

        DateTime now = DateTime.Now;
        IEnumerable<AnimalRecord> recordsFromCsv = new List<AnimalRecord>();

        using (StreamReader reader = new StreamReader(fileFullPath))
        {
            using (CsvReader csvReader = new CsvReader(reader, csvConfiguration))
            {
                recordsFromCsv = csvReader.GetRecords<AnimalRecord>().ToList();
            }
        }

        Dictionary<int, Animal> recordsFromDb = await _context.Animals.ToDictionaryAsync(f => f.Id);

        foreach (AnimalRecord csvRecord in recordsFromCsv)
        {
            // value from CSV is invalid
            if (!csvRecord.Id.HasValue || string.IsNullOrEmpty(csvRecord.Name))
            {
                skippedRows++;
                continue;
            }

            // value from CSV already exists
            if (recordsFromDb.ContainsKey(csvRecord.Id.Value))
            {
                skippedRows++;
                continue;
            }

            Animal recordToAdd = _mapper.Map<Animal>(csvRecord);
            recordToAdd.DateCreated = now;
            recordToAdd.DateUpdated = now;

            _context.Animals.Add(recordToAdd);
            addedRows++;
        }

        using var transaction = _context.Database.BeginTransaction();

        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Animals ON");
        await _context.SaveChangesAsync();
        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Animals OFF");

        transaction.Commit();

        return new JsonResult(new
        {
            AddedRows = addedRows,
            SkippedRows = skippedRows
        });
    }
}
