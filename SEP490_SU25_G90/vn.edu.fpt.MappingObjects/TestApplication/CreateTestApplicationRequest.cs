using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication
{
    public class CreateTestApplicationRequest
    {
        public CreateTestApplicationRequest()
        {
        }

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

        [Required(ErrorMessage = "Điểm thi lý thuyết không được để trống")]
        [Display(Name = "Điểm thi lý thuyết")]
        [Range(0, int.MaxValue, ErrorMessage = "Điểm thi phải lớn hơn 0")]
        public int? TheoryScore { get; set; }

        [Required(ErrorMessage = "Điểm thi mô phỏng không được để trống")]
        [Display(Name = "Điểm thi mô phỏng")]
        [Range(0, int.MaxValue, ErrorMessage = "Điểm thi phải lớn hơn 0")]
        public int? SimulationScore { get; set; }

        [Required(ErrorMessage = "Điểm thi sa hình không được để trống")]
        [Display(Name = "Điểm thi sa hình")]
        [Range(0, int.MaxValue, ErrorMessage = "Điểm thi phải lớn hơn 0")]
        public int? ObstacleScore { get; set; }

        [Required(ErrorMessage = "Điểm thi đường trường không được để trống")]
        [Display(Name = "Điểm thi đường trường")]
        [Range(0, int.MaxValue, ErrorMessage = "Điểm thi phải lớn hơn 0")]
        public int? PracticalScore { get; set; }

        [Display(Name = "Tài liệu đính kèm")]
        public IFormFile? Attachment { get; set; }

        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

    }
}
