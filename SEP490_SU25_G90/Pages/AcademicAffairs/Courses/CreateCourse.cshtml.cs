using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.CourseService;
using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.Courses
{
    [Authorize(Roles = "academic affairs")]
    public class CreateCourseModel : PageModel
    {
        private readonly ICourseService _courseService;

        public CreateCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseFormRequest Input { get; set; } = new();

        public List<SelectListItem> LicenceTypes { get; set; } = new();
        public string? GeneratedCourseName { get; set; }

        public async Task OnGetAsync()
        {
            await LoadLicenceTypesAsync();
            if (!Input.LicenceTypeId.HasValue && LicenceTypes.Any())
            {
                if (byte.TryParse(LicenceTypes.First().Value, out var firstId))
                {
                    Input.LicenceTypeId = firstId;
                }
            }
            if (Input.LicenceTypeId.HasValue)
            {
                GeneratedCourseName = await _courseService.GenerateCourseNameAsync(Input.LicenceTypeId.Value);
            }
        }

        public async Task<IActionResult> OnGetGenerateNameAsync(byte licenceTypeId)
        {
            var name = await _courseService.GenerateCourseNameAsync(licenceTypeId);
            return new JsonResult(new { name });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadLicenceTypesAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.StartDate.HasValue && Input.EndDate.HasValue && Input.StartDate > Input.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
                return Page();
            }

            try
            {
                if (!Input.LicenceTypeId.HasValue)
                {
                    ModelState.AddModelError(string.Empty, "Vui lòng chọn loại bằng.");
                    return Page();
                }

                // Tự sinh tên khóa học theo quy tắc BH{seq}-{licenceCode}
                var generatedName = await _courseService.GenerateCourseNameAsync(Input.LicenceTypeId.Value);

                var entity = new Course
                {
                    CourseName = generatedName,
                    LicenceTypeId = Input.LicenceTypeId,
                    StartDate = Input.StartDate.HasValue ? DateOnly.FromDateTime(Input.StartDate.Value) : null,
                    EndDate = Input.EndDate.HasValue ? DateOnly.FromDateTime(Input.EndDate.Value) : null
                };

                await _courseService.CreateAsync(entity);
                TempData["SuccessMessage"] = "Tạo khóa học thành công.";
                return RedirectToPage("./ListCourses");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private async Task LoadLicenceTypesAsync()
        {
            var types = await _courseService.GetAllLicenceTypesAsync();
            LicenceTypes = types.Select(t => new SelectListItem
            {
                Value = t.LicenceTypeId.ToString(),
                Text = t.LicenceCode
            }).ToList();
        }

        public class CourseFormRequest
        {
            // CourseName được tự sinh phía server, không bind từ form
            public string? CourseName { get; set; }

            [Required(ErrorMessage = "Loại bằng là bắt buộc.")]
            public byte? LicenceTypeId { get; set; }

            [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc.")]
            [DataType(DataType.Date)]
            public DateTime? StartDate { get; set; }

            [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc.")]
            [DataType(DataType.Date)]
            public DateTime? EndDate { get; set; }
        }
    }
}

