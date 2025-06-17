using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TeknorixJobsOpenings.Data;
using TeknorixJobsOpenings.Modals.Entities;

namespace TeknorixJobsOpenings.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public DepartmentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _context.Department
                .Select(d => new
                {
                    d.Id,
                    d.Title
                }).ToListAsync();

            return Ok(departments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                return BadRequest(new { Message = "Title is required." });

            var department = new Department { Title = model.Title };
            _context.Department.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllDepartments), new { id = department.Id }, department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department model)
        {
            var department = await _context.Department.FindAsync(id);
            if (department == null)
                return NotFound(new { Message = $"Department with ID {id} not found." });

            department.Title = model.Title;
            await _context.SaveChangesAsync();

            return Ok(department);
        }
    }
}
