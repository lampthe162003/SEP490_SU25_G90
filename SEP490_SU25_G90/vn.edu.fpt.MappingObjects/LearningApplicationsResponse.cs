namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class LearningApplicationsResponse
    {
        // Mã hồ sơ học
        public int LearningId { get; set; }

        // Thông tin học viên
        public int? LearnerId { get; set; }
        public string? LearnerFullName { get; set; }
        public string? LearnerCccdNumber { get; set; }

        // Loại bằng
        public byte? LicenceTypeId { get; set; }
        public string? LicenceTypeName { get; set; }

        // Thông tin giảng viên
        public int? InstructorId { get; set; }
        public string? InstructorFullName { get; set; }

        // Ngày đăng ký
        public DateTime? SubmittedAt { get; set; }

        // Trạng thái hồ sơ
        public byte? LearningStatus { get; set; }
        public string? LearningStatusName { get; set; }

        // Các trường khác nếu cần
        // public DateTime? AssignedAt { get; set; }
        // public bool? TestEligibility { get; set; }
    }
}