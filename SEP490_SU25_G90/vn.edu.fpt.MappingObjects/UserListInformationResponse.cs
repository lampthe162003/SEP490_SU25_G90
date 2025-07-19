using SEP490_SU25_G90.vn.edu.fpt.Models;
using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class UserListInformationResponse
    {
        public int UserId { get; set; }

        public string? Email { get; set; }

        [Display(Name = "Họ Tên")]
        public string? FullName { get; set; }

        [Display(Name = "Ngày Sinh")]
        public DateOnly? Dob { get; set; }

        [Display(Name = "Giới Tính")]
        public bool? Gender { get; set; }

        [Display(Name = "Số Điện Thoại")]
        public string? Phone { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
