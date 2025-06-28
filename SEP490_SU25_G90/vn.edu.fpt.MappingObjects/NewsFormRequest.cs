using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class NewsFormRequest
    {
        public int NewsId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [MaxLength(500)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string NewsContent { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        public string? OldImagePath { get; set; }
    }
}
