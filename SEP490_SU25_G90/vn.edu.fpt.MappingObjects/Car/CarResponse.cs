using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Car
{
    public class CarResponse
    {
        public int CarId { get; set; }
        
        [Display(Name = "Loại xe thi")]
        public string? CarMake { get; set; }
        
        [Display(Name = "Mẫu xe")]
        public string? CarModel { get; set; }
        
        [Display(Name = "Biển số xe")]
        public string? LicensePlate { get; set; }
        
        // Thông tin thống kê
        public int TotalAssignments { get; set; }
        public int ActiveAssignments { get; set; }
        public bool IsCurrentlyRented { get; set; }
        public string? CurrentInstructorName { get; set; }
    }
} 