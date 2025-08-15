using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment
{
    public class CarRentalRequest
    {
        public int CarId { get; set; }
        
        public int InstructorId { get; set; }
        
        public int SlotId { get; set; }
        
        [DataType(DataType.Date)]
        public DateOnly ScheduleDate { get; set; }
        
        public byte? CarStatus { get; set; } 
    }
} 