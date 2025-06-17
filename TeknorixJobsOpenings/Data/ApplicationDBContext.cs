using Microsoft.EntityFrameworkCore;
using TeknorixJobsOpenings.Modals.Entities;

namespace TeknorixJobsOpenings.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<JobOpening> JobOpenings { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
