using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class InstructorListInformationResponse
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();
        public DateOnly? Dob { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public string? ProfileImageUrl { get; set; }

        // Address information
        public string? AddressDisplay { get; set; }

        // CCCD information
        public string? CccdNumber { get; set; }
        public string? CccdImageUrl { get; set; }
        public string? CccdImageUrlMs { get; set; }

        // Specializations - các loại bằng giảng viên có thể dạy
        public List<LicenceTypeResponse> Specializations { get; set; } = new List<LicenceTypeResponse>();

        // Số học viên hiện tại
        public int StudentCount { get; set; }
    }

    public class LicenceTypeResponse
    {
        public byte LicenceTypeId { get; set; }
        public string LicenceCode { get; set; } = null!;
    }

    public class UpdateInstructorRequest
    {
        public int UserId { get; set; }
        
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
        
        [Required(ErrorMessage = "Họ là bắt buộc")]
        [StringLength(50, ErrorMessage = "Họ không được quá 50 ký tự")]
        public string? FirstName { get; set; }
        
        [StringLength(50, ErrorMessage = "Tên đệm không được quá 50 ký tự")]
        public string? MiddleName { get; set; }
        
        [Required(ErrorMessage = "Tên là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên không được quá 50 ký tự")]
        public string? LastName { get; set; }
        
        public DateOnly? Dob { get; set; }
        public bool? Gender { get; set; }
        
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? Phone { get; set; }
        
        [RegularExpression(@"^(\d{12})?$", ErrorMessage = "Số CCCD phải có đúng 12 chữ số và chỉ chứa số")]
        public string? CccdNumber { get; set; }
        
        public string? CccdImageFront { get; set; }
        
        public string? CccdImageBack { get; set; }
        
        public List<byte> SelectedSpecializations { get; set; } = new List<byte>();
    }
}