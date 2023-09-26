using FarmsAPI.DTO;
using FarmsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmsAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class HerdController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HerdController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task GetHerdDetails(int id)
    {
        Herd? result = await _context.Herds.FirstOrDefaultAsync(h => h.Id == id);
    }

    [HttpGet(Name = "GetHerds")]
    public async Task GetHerds([FromQuery] object searchOptions)
    {

    }

    [HttpPost(Name = "CreateHerd")]
    public async Task<IActionResult> CreateHerd(HerdDto herdDto)
    {
        if (string.IsNullOrEmpty(herdDto.Name))
        {
            return BadRequest("Name is required.");
        }

        var newHerd = new Herd()
        {
            Name = herdDto.Name,
            Address = herdDto.Address,
            City = herdDto.City,
            Region = herdDto.Region,
            Country = herdDto.Country,
            DateCreated = DateTime.Now
        };

        await _context.AddAsync(newHerd);
        await _context.SaveChangesAsync();

        return CreatedAtAction("someName", new { id = herdDto.Id }, herdDto);
    }


    [HttpPut(Name = "UpdateHerd")]
    public async Task UpdateHerd(HerdDto herdDto)
    {
        var herdToUpdate = await _context.Herds.FirstOrDefaultAsync(h => h.Id == herdDto.Id);

        if (herdToUpdate != null)
        {
            if (!string.IsNullOrEmpty(herdDto.Name))
                herdToUpdate.Name = herdDto.Name;

            herdToUpdate.Address = herdDto.Address;
            herdToUpdate.City = herdDto.City;
            herdToUpdate.Region = herdDto.Region;
            herdToUpdate.Country = herdDto.Country;

            herdToUpdate.DateUpdated = DateTime.Now;

            _context.Update(herdToUpdate);
            await _context.SaveChangesAsync();
        }
    }

    [HttpDelete(Name = "DeleteHerd")]
    public async Task DeleteHerd(int id)
    {
        var herdToDelete = await _context.Herds.FirstOrDefaultAsync(h => h.Id == id);

        if (herdToDelete != null)
        {
            _context.Herds.Remove(herdToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
