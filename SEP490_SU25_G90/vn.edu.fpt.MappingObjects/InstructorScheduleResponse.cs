namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class InstructorScheduleResponse
    {
        public DateOnly ScheduleDate { get; set; }
        public int SlotId { get; set; }
        public string? ClassName { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;

    }
}
