using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial
{
    public class LearningMaterialFormRequest
    {
        public int MaterialId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chọn loại bằng")]
        public byte LicenceTypeId { get; set; }

        public IFormFile? File { get; set; }

        public string? OldFilePath { get; set; }
    }
}
