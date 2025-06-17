namespace TeknorixJobsOpenings.Modals.Entities
{
    public class JobOpening
    {
        public int Id { get; set; }
        public string Code { get; set; } // e.g., "JOB-01"
        public string Title { get; set; }
        public string Description { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;
        public DateTime ClosingDate { get; set; }
    }

    public class Location
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int Zip { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
