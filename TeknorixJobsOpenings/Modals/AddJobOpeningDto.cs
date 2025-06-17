using TeknorixJobsOpenings.Modals.Entities;

namespace TeknorixJobsOpenings.Modals
{
    public class AddJobOpeningDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int LocationId { get; set; }

        public int DepartmentId { get; set; }

        public DateTime ClosingDate { get; set; }
    }

    public class JobListRequestDto
    {
        public string? Q { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? LocationId { get; set; }
        public int? DepartmentId { get; set; }
    }

    public class JobListItemDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime PostedDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }

    public class JobDetailsDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public LocationDto Location { get; set; } = new();
        public DepartmentDto Department { get; set; } = new();
        public DateTime PostedDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }

    public class LocationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int Zip { get; set; }
    }

    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class CreateOrUpdateDepartmentDto
    {
        public string Title { get; set; } = string.Empty;
    }
}
