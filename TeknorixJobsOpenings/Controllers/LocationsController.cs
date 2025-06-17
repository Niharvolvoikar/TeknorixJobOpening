using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknorixJobsOpenings.Modals.Entities;
using TeknorixJobsOpenings.Data;
using TeknorixJobsOpenings.Modals;

namespace TeknorixJobsOpenings.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public LocationsController(ApplicationDBContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLocations()
        {
            var locations = await _context.Location.ToListAsync();
            return Ok(locations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] LocationinsertDto location)
        {
            try
            {
                var locationInsert = new Location()
                {
                    Country = location.Country,
                    City = location.City,
                    State = location.State,
                    Title = location.Title,
                    Zip = location.Zip, 
                };
                _context.Location.Add(locationInsert);
                await _context.SaveChangesAsync();
                return Ok(location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating location", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] Location updatedLocation)
        {
            var location = await _context.Location.FindAsync(id);
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
