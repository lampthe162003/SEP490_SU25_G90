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
        [Display(Name = "Mật khẩu")]
        public String PasswordHash { get; set; } = default!;

        [Required(ErrorMessage = "Họ không được để trống")]
        [Display(Name = "Tên họ")]
        public string FirstName { get; set; } = default!;

        [Display(Name = "Tên đệm")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [Display(Name = "Tên Riêng")]
        public string LastName { get; set; } = default!;

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [Display(Name = "Ngày sinh")]
        public DateOnly Dob { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Hãy chọn giới tính hợp lệ")]
        public bool Gender { get; set; }

        [Display(Name = "SĐT")]
        public string? Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        public virtual Address? Address { get; set; }

        public virtual ICollection<InstructorSpecialization> InstructorSpecializations { get; set; } = new List<InstructorSpecialization>();

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
