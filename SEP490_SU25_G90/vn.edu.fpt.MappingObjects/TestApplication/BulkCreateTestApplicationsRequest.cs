using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication
{
    public class BulkCreateTestApplicationsRequest
    {
        [Required(ErrorMessage = "Vui lòng chọn ít nhất một học viên")]
        public List<int> LearningApplicationIds { get; set; } = new();

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày nộp hồ sơ không được để trống")]
        [Display(Name = "Ngày nộp hồ sơ")]
        public DateOnly? SubmitProfileDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày thi không được để trống")]
        [Display(Name = "Ngày thi")]
        public DateOnly? ExamDate { get; set; }

        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }
    }
}

