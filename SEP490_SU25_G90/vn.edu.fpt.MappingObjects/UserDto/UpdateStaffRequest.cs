using SEP490_SU25_G90.vn.edu.fpt.Models;
using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto
{
    public class UpdateStaffRequest
    {
        public int UserId { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string Fullname { get; set; } = default!;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [MaxLength(10)]
        public string Phone { get; set; } = default!;

        [Required(ErrorMessage = "Bạn phải chọn giới tính")]
        public bool Gender { get; set; } = default!;

        [Required(ErrorMessage = "Bạn phải chọn ngày sinh")]
        public DateOnly Dob { get; set; }

        private Cccd _cccd = new Cccd();

        public virtual Cccd Cccd { get => _cccd; set => _cccd = value ?? new Cccd(); }

    }
}
