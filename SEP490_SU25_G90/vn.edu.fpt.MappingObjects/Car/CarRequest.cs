using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Car
{
    public class CarRequest
    {
        [Required(ErrorMessage = "Loại xe thi là bắt buộc")]
        [StringLength(50, ErrorMessage = "Loại xe thi không được vượt quá 50 ký tự")]
        [Display(Name = "Loại xe thi")]
        public string CarMake { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Mẫu xe là bắt buộc")]
        [StringLength(50, ErrorMessage = "Mẫu xe không được vượt quá 50 ký tự")]
        [Display(Name = "Mẫu xe")]
        public string CarModel { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Biển số xe là bắt buộc")]
        [StringLength(13, ErrorMessage = "Biển số xe không được vượt quá 13 ký tự")]
        [Display(Name = "Biển số xe")]
        public string LicensePlate { get; set; } = string.Empty;
    }
} 