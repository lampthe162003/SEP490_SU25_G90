using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication
{
    public class CreatUpdateTestApplicationRequest
    {
        public CreatUpdateTestApplicationRequest()
        {
        }

        public string? FullName { get; set; }

        public string? DateOfBirth { get; set; }

        public string? LicenseType { get; set; }

        [Display(Name = "CCCD")]
        public string? CCCD { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn hồ sơ khóa học cần thi")]
        public int? LearningApplicationId { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày nộp hồ sơ không được để trống")]
        [Display(Name = "Ngày nộp hồ sơ")]
        public DateOnly? SubmitProfileDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày thi không được để trống")]
        [Display(Name = "Ngày thi")]
        public DateOnly? ExamDate { get; set; }

        [Display(Name = "Điểm thi lý thuyết")]
        public int? TheoryScore { get; set; }

        [Display(Name = "Điểm thi mô phỏng")]
        public int? SimulationScore { get; set; }

        [Display(Name = "Điểm thi sa hình")]
        public int? ObstacleScore { get; set; }

        [Display(Name = "Điểm thi đường trường")]
        public int? PracticalScore { get; set; }

        [Display(Name = "Tài liệu đính kèm")]
        public IFormFile? Attachment { get; set; }

        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        public string? ResultImageUrl;

    }
}
