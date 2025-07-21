using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment
{
    public class CarAssignmentInformationResponse
    {
        public int AssignmentId { get; set; }

        public int CarId { get; set; }

        public int InstructorId { get; set; }

        public int SlotId { get; set; }

        public DateOnly? ScheduleDate { get; set; }

        public bool? CarStatus { get; set; }

        public virtual Car Car { get; set; } = null!;

        public virtual CarBorrowerInformationResponse Instructor { get; set; } = null!;

        public virtual ScheduleSlot Slot { get; set; } = null!;
    }
}
