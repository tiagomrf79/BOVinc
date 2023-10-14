using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using FarmsAPI.DbContexts;
using FarmsAPI.Models;
using FarmsAPI.Models.csv;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FarmsAPI.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class SeedController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SeedController> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly IMapper _mapper;

    public SeedController(ApplicationDbContext context, ILogger<SeedController> logger, IWebHostEnvironment env, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _env = env;
        _mapper = mapper;
    }

    [HttpPut]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> SeedFarmsData()
    {
        CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-PT"))
        {
            HasHeaderRecord = true,
            Delimiter = ";"
        };

        string fileFullPath = Path.Combine(_env.ContentRootPath, "Data/farms_dummy_dataset.csv");

        int addedRows = 0;
        int skippedRows = 0;

        DateTime now = DateTime.Now;
        IEnumerable<FarmRecord> recordsFromCsv = new List<FarmRecord>();

        using (StreamReader reader = new StreamReader(fileFullPath))
        {
            using (CsvReader csvReader = new CsvReader(reader, csvConfiguration))
            {
                recordsFromCsv = csvReader.GetRecords<FarmRecord>().ToList();
            }
        }

        Dictionary<int, Farm> recordsFromDb = await _context.Farms.ToDictionaryAsync(f => f.Id);

        foreach (FarmRecord csvRecord in recordsFromCsv)
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

            Farm recordToAdd = _mapper.Map<Farm>(csvRecord);
            recordToAdd.DateCreated = now;
            recordToAdd.DateUpdated = now;

            _context.Farms.Add(recordToAdd);
            addedRows++;
        }

        using var transaction =_context.Database.BeginTransaction();

        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Farms ON");
        await _context.SaveChangesAsync();
        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Farms OFF");

        transaction.Commit();

        return new JsonResult(new
        {
            AddedRows = addedRows,
            SkippedRows = skippedRows
        });
    }
}
