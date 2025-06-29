using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class LearningApplicationsResponse : IValidatableObject
    {
        public int LearningId { get; set; }
        public int? LearnerId { get; set; }

        [Required(ErrorMessage = "Họ tên học viên là bắt buộc")]
        public string? LearnerFullName { get; set; }

        [Required(ErrorMessage = "CCCD là bắt buộc")]
        public string? LearnerCccdNumber { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime? LearnerDob { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        public string? LearnerPhone { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? LearnerEmail { get; set; }

        public string? LearnerCccdImageUrl { get; set; }
        public string? LearnerHealthCertImageUrl { get; set; }

        [Required(ErrorMessage = "Loại bằng là bắt buộc")]
        public byte? LicenceTypeId { get; set; }

        public string? LicenceTypeName { get; set; }

        [Required(ErrorMessage = "Giảng viên là bắt buộc")]
        public int? InstructorId { get; set; }

        public string? InstructorFullName { get; set; }

        // Danh sách lớp học viên đang tham gia
        public List<LearnerClassInfo> LearnerClasses { get; set; } = new List<LearnerClassInfo>();

        [Required(ErrorMessage = "Ngày đăng ký là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime? SubmittedAt { get; set; }

        public byte? LearningStatus { get; set; }
        public string? LearningStatusName { get; set; }

        public int? TheoryScore { get; set; }
        public int? SimulationScore { get; set; }
        public int? ObstacleScore { get; set; }
        public int? PracticalScore { get; set; }

        public int? TheoryPassScore { get; set; }
        public int? SimulationPassScore { get; set; }
        public int? ObstaclePassScore { get; set; }
        public int? PracticalPassScore { get; set; }

        public string? Note { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (LearnerDob != null && SubmittedAt != null)
            {
                if (LearnerDob >= SubmittedAt)
                {
                    yield return new ValidationResult("Ngày sinh phải trước ngày đăng ký.",
                        new[] { nameof(LearnerDob), nameof(SubmittedAt) });
                }
            }
        }
    }
}
