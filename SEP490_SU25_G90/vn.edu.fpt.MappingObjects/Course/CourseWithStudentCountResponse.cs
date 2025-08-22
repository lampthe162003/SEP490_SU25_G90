namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course
{
    public class CourseWithStudentCountResponse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public byte? LicenceTypeId { get; set; }
        public int StudentCount { get; set; }
        public string DisplayText => $"{CourseName} ({StudentCount} sinh viên)";
    }
}
