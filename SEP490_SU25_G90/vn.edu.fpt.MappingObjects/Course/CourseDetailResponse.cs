using System;
using System.Collections.Generic;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course
{
    /// <summary>
    /// Thông tin chi tiết khóa học kèm danh sách lớp
    /// </summary>
    public class CourseDetailResponse
    {
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? LicenceCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalClasses { get; set; }
        public int TotalStudents { get; set; }
        public IEnumerable<ClassListResponse> Classes { get; set; } = new List<ClassListResponse>();
    }
}

