using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Car
{
    public class CarSearchRequest
    {
        [Display(Name = "Loại xe thi")]
        public string? CarMake { get; set; }
        
        [Display(Name = "Mẫu xe")]
        public string? CarModel { get; set; }
        
        [Display(Name = "Biển số xe")]
        public string? LicensePlate { get; set; }
        
        [Display(Name = "Trạng thái")]
        public bool? IsRented { get; set; }
    }
} 