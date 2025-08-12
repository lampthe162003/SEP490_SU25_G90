using System;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course
{
    /// <summary>
    /// Response DTO cho danh sách khóa học
    /// </summary>
    public class CourseListResponse
    {
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? LicenceCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalClasses { get; set; }
        public int TotalStudents { get; set; }
    }
}

