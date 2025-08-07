namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class
{
    /// <summary>
    /// Response DTO cho danh sách lớp học
    /// </summary>
    public class ClassListResponse
    {
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public string? InstructorName { get; set; }
        public string? LicenceCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
    }
} 