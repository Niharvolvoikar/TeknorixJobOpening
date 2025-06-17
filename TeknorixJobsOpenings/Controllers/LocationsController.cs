using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknorixJobsOpenings.Modals.Entities;
using TeknorixJobsOpenings.Data;

namespace TeknorixJobsOpenings.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public LocationsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            return Ok(locations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] Location location)
        {
            try
            {
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetLocations), new { id = location.Id }, location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating location", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] Location updatedLocation)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
                return NotFound();

            location.Title = updatedLocation.Title;
            location.City = updatedLocation.City;
            location.State = updatedLocation.State;
            location.Country = updatedLocation.Country;
            location.Zip = updatedLocation.Zip;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating location", details = ex.Message });
            }
        }
    }
}
