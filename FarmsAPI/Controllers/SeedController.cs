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
    [ResponseCache(NoStore = true)]
    public async Task<IActionResult> FarmsData()
    {
        using var transaction =_context.Database.BeginTransaction();
        await _context.Farms.ExecuteDeleteAsync();

        CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.GetCultureInfo("pt-PT"))
        {
            HasHeaderRecord = true,
            Delimiter = ";"
        };

        string fileFullPath = Path.Combine(_env.ContentRootPath, "Data/farms_dummy_dataset.csv");
        using StreamReader reader = new StreamReader(fileFullPath);
        using CsvReader csvReader = new CsvReader(reader, csvConfiguration);

        DateTime now = DateTime.Now;
        int skippedRows = 0;

        IEnumerable<FarmRecord> records = csvReader.GetRecords<FarmRecord>();

        foreach (FarmRecord record in records)
        {
            if (!record.Id.HasValue || string.IsNullOrEmpty(record.Name))
            {
                skippedRows++;
                continue;
            }

            Farm recordToAdd = _mapper.Map<Farm>(record);
            recordToAdd.DateCreated = now;
            recordToAdd.DateUpdated = now;

            _context.Farms.Add(recordToAdd);
        }

        await _context.SaveChangesAsync();
        transaction.Commit();

        return new JsonResult(new
        {
            Farms = _context.Farms.Count(),
            SkippedRows = skippedRows
        });
    }
}
