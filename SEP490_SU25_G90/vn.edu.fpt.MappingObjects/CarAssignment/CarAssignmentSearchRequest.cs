using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment
{
    public class CarAssignmentSearchRequest
    {
        [Display(Name = "Loại xe thi (Car Make)")]
        public string? CarMake { get; set; }
        
        [Display(Name = "Ngày mượn xe")]
        [DataType(DataType.Date)]
        public DateOnly? ScheduleDate { get; set; }
        
        [Display(Name = "Ca học")]
        public int? SlotId { get; set; }
        
        [Display(Name = "Trạng thái")]
        public bool? CarStatus { get; set; }
        
        [Display(Name = "Chỉ hiển thị xe của tôi")]
        public bool ShowMyReservationsOnly { get; set; } = false;
    }
} 