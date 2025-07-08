using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class LearningApplicationsResponse : IValidatableObject
    {
        public int LearningId { get; set; }
        public int? LearnerId { get; set; }

        
        public string? LearnerFullName { get; set; }

        
        public string? LearnerCccdNumber { get; set; }

        
        public DateTime? LearnerDob { get; set; }

        
        public string? LearnerPhone { get; set; }

        
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

        
    }
}
