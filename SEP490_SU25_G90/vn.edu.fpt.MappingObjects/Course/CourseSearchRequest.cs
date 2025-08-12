using System;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course
{
    /// <summary>
    /// Request DTO cho tìm kiếm khóa học
    /// </summary>
    public class CourseSearchRequest
    {
        public string? CourseName { get; set; }
        public byte? LicenceTypeId { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

