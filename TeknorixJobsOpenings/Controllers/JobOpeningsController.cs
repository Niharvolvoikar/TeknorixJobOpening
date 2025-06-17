using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeknorixJobsOpenings.Data;
using TeknorixJobsOpenings.Modals;
using TeknorixJobsOpenings.Modals.Entities;

namespace TeknorixJobsOpenings.Controllers
{
    [Route("api/v1/jobs")]
    [ApiController]
    public class JobOpeningsController : ControllerBase
    {
        private readonly ApplicationDBContext dBContext;
        public JobOpeningsController(ApplicationDBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        [HttpGet]
        public IActionResult GetJobopenings() 
        {
            var allopenings = dBContext.JobOpenings.ToList();
            return Ok(allopenings);
        }

        [HttpPost]
        public async Task<IActionResult> AddJobOpening(AddJobOpeningDto addJobOpening)
        {
            try
            {
                int nextId = 1;

                var lastJob = await dBContext.JobOpenings
                    .OrderByDescending(j => j.Id)
                    .FirstOrDefaultAsync();

                if (lastJob != null)
                    nextId = lastJob.Id + 1;

                if (addJobOpening.ClosingDate <= DateTime.UtcNow.Date)
                {
                    return BadRequest(new { Message = "Closing date must be a future date." });
                }

                var jobOpening = new JobOpening
                {
                    Code = "JOBs-" + nextId.ToString("D5"),
                    Title = addJobOpening.Title,
                    Description = addJobOpening.Description,
                    LocationId = addJobOpening.LocationId,
                    DepartmentId = addJobOpening.DepartmentId,
                    PostedDate = DateTime.UtcNow,
                    ClosingDate = addJobOpening.ClosingDate
                };

                dBContext.JobOpenings.Add(jobOpening);
                await dBContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetJobOpeningById), new { id = jobOpening.Id }, jobOpening.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while adding the job opening.",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobOpening(int id, [FromBody] AddJobOpeningDto updatedJob)
        {
            try
            {
                var job = await dBContext.JobOpenings.FindAsync(id);
                if (job == null)
                    return NotFound(new { Message = $"Job with ID {id} not found." });

                if (updatedJob.ClosingDate <= DateTime.UtcNow.Date)
                    return BadRequest(new { Message = "Closing date must be a future date." });

                job.Title = updatedJob.Title;
                job.Description = updatedJob.Description;
                job.LocationId = updatedJob.LocationId;
                job.DepartmentId = updatedJob.DepartmentId;
                job.ClosingDate = updatedJob.ClosingDate;

                await dBContext.SaveChangesAsync();

                return Ok(job.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the job.", Details = ex.Message });
            }
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetJobList([FromBody] JobListRequestDto request)
        {
            try
            {
                var query = dBContext.JobOpenings
                    .Include(j => j.Location)
                    .Include(j => j.Department)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Q))
                {
                    query = query.Where(j => j.Title.Contains(request.Q));
                }

                if (request.LocationId.HasValue)
                {
                    query = query.Where(j => j.LocationId == request.LocationId.Value);
                }

                if (request.DepartmentId.HasValue)
                {
                    query = query.Where(j => j.DepartmentId == request.DepartmentId.Value);
                }

                var total = await query.CountAsync();

                var data = await query
                    .OrderByDescending(j => j.PostedDate)
                    .Skip((request.PageNo - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(j => new JobListItemDto
                    {
                        Id = j.Id,
                        Code = j.Code,
                        Title = j.Title,
                        Location = j.Location.Title,
                        Department = j.Department.Title,
                        PostedDate = j.PostedDate,
                        ClosingDate = j.ClosingDate
                    })
                    .ToListAsync();

                return Ok(new
                {
                    total,
                    data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving job listings.", Details = ex.Message });
            }
        }

        [HttpGet("api/v1/jobs/{id}")]
        public async Task<IActionResult> GetJobOpeningByIdd(int id)
        {
            try
            {
                var job = await dBContext.JobOpenings
                    .Include(j => j.Location)
                    .Include(j => j.Department)
                    .FirstOrDefaultAsync(j => j.Id == id);

                if (job == null)
                    return NotFound(new { Message = $"Job with ID {id} not found." });

                var jobDetails = new JobDetailsDto
                {
                    Id = job.Id,
                    Code = job.Code,
                    Title = job.Title,
                    Description = job.Description,
                    PostedDate = job.PostedDate,
                    ClosingDate = job.ClosingDate,
                    Location = new LocationDto
                    {
                        Id = job.Location.Id,
                        Title = job.Location.Title,
                        City = job.Location.City,
                        State = job.Location.State,
                        Country = job.Location.Country,
                        Zip = job.Location.Zip
                    },
                    Department = new DepartmentDto
                    {
                        Id = job.Department.Id,
                        Title = job.Department.Title
                    }
                };

                return Ok(jobDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving job details.", Details = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobOpeningById(int id)
        {
            var jobOpening = await dBContext.JobOpenings
                .Include(j => j.Location)
                .Include(j => j.Department)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (jobOpening == null)
                return NotFound();

            return Ok(jobOpening);
        }
    }
}
