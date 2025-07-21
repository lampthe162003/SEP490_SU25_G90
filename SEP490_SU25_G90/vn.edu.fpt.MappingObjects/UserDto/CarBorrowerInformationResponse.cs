using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto
{
    public class CarBorrowerInformationResponse
    {
        public int UserId { get; set; }

        public string? Email { get; set; }

        [Display(Name = "Họ Tên")]
        public string? FullName { get; set; }
    }
}
