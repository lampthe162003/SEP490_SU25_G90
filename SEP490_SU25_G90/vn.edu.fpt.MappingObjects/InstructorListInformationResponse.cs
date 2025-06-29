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

    public class LearnerClassInfo
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public string LicenceCode { get; set; } = string.Empty;
    }

    public class LicenceProgress
    {
        public int LearningId { get; set; }
        public byte? LicenceTypeId { get; set; }
        public string LicenceTypeName { get; set; } = string.Empty;
        public DateTime? SubmittedAt { get; set; }
        public byte? LearningStatus { get; set; }
        public string LearningStatusName { get; set; } = string.Empty;
        public int? TheoryScore { get; set; }
        public int? SimulationScore { get; set; }
        public int? ObstacleScore { get; set; }
        public int? PracticalScore { get; set; }
        public bool IsCompleted { get; set; }
        public string StatusBadgeClass { get; set; } = string.Empty;
    }

    public class LearnerSummaryResponse
    {
        public int? LearnerId { get; set; }
        public string LearnerFullName { get; set; } = string.Empty;
        public string LearnerCccdNumber { get; set; } = string.Empty;
        public DateTime? LearnerDob { get; set; }
        public string LearnerPhone { get; set; } = string.Empty;
        public string LearnerEmail { get; set; } = string.Empty;
        public string LearnerCccdImageUrl { get; set; } = string.Empty;
        public string LearnerHealthCertImageUrl { get; set; } = string.Empty;
        public List<LicenceProgress> LicenceProgresses { get; set; } = new List<LicenceProgress>();
        public DateTime? LatestSubmittedAt { get; set; }
        public string OverallStatus { get; set; } = string.Empty;
        public int CompletedLicences { get; set; }
        public int TotalLicences { get; set; }
    }

    public class LearnerUserResponse
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CccdNumber { get; set; } = string.Empty;
        public DateOnly? Dob { get; set; }
        public bool? Gender { get; set; }
        public string CccdImageUrl { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}