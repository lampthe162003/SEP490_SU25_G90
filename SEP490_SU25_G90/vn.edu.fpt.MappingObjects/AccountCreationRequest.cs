using SEP490_SU25_G90.vn.edu.fpt.Models;
using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class AccountCreationRequest
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ. Vui lòng thử lại.")]
        public String Email { get; set; } = default!;

        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public String PasswordHash { get; set; } = default!;

        public string? ProfileImageUrl { get; set; }

        [Required(ErrorMessage = "Họ không được để trống")]
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateOnly? Dob { get; set; }

        public bool? Gender { get; set; }

        public string? Phone { get; set; }

        public virtual Address? Address { get; set; }

        public virtual Cccd? Cccd { get; set; }

        public virtual HealthCertificate? HealthCertificate { get; set; }

        public virtual ICollection<InstructorSpecialization> InstructorSpecializations { get; set; } = new List<InstructorSpecialization>();

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
