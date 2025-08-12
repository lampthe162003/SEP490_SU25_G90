namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class
{
    /// <summary>
    /// Request DTO cho tìm kiếm lớp học
    /// </summary>
    public class ClassSearchRequest
    {
        public string? ClassName { get; set; }
        public byte? LicenceTypeId { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? InstructorId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
} 