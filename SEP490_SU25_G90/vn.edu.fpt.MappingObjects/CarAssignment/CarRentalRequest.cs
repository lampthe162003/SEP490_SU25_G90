using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment
{
    public class CarRentalRequest
    {
        [Required(ErrorMessage = "Xe là bắt buộc")]
        public int CarId { get; set; }
        
        [Required(ErrorMessage = "Giáo viên là bắt buộc")]
        public int InstructorId { get; set; }
        
        [Required(ErrorMessage = "Ca học là bắt buộc")]
        public int SlotId { get; set; }
        
        [Required(ErrorMessage = "Ngày mượn xe là bắt buộc")]
        [DataType(DataType.Date)]
        public DateOnly ScheduleDate { get; set; }
        
        public bool CarStatus { get; set; } = true; // Mặc định là đã được mượn
    }
} 