using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class InstructorScheduleResponse
    {
        public DateOnly ScheduleDate { get; set; }
        public int SlotId { get; set; }
        public int ClassId { get; set; }
        public int ClassTimeId { get; set; } // Added for proper attendance tracking
        public string? ClassName { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public byte DayOfWeek { get; set; }
        public string? CourseName { get; set; } = string.Empty;
        public DateOnly? CourseStartDate { get; set; }
        public DateOnly? CourseEndDate { get; set; }
        
        // Attendance information
        public int TotalStudents { get; set; }
        public int PresentStudents { get; set; }
        public int AbsentStudents { get; set; }
        public double AttendanceRate { get; set; }
        public List<StudentAttendanceInfo> StudentAttendances { get; set; } = new();
    }

    public class StudentAttendanceInfo
    {
        public int LearnerId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public bool? AttendanceStatus { get; set; }
        public double? PracticalDurationHours { get; set; }
        public double? PracticalDistance { get; set; }
        public string? Note { get; set; } = string.Empty;
    }
}
