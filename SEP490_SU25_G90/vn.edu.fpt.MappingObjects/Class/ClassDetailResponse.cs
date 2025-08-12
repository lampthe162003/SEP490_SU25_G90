namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class
{
    /// <summary>
    /// Response DTO cho chi tiết lớp học
    /// </summary>
    public class ClassDetailResponse
    {
        public int ClassId { get; set; }
        public string? CourseName { get; set; }
        public string? ClassName { get; set; }
        public int? InstructorId { get; set; }
        public string? InstructorName { get; set; }
        public string? InstructorPhone { get; set; }
        public string? InstructorEmail { get; set; }
        public string? LicenceCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public List<ClassMemberResponse> Members { get; set; } = new List<ClassMemberResponse>();
    }

    /// <summary>
    /// Response DTO cho thành viên lớp học
    /// </summary>
    public class ClassMemberResponse
    {
        public int UserId { get; set; }
        public string? StudentCode { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? LearningStatus { get; set; }
        public DateTime? JoinDate { get; set; }
    }
} 