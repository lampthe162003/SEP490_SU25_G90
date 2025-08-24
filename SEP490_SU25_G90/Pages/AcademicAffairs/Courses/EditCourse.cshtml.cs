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
    public class EditCourseModel : PageModel
    {
        private readonly ICourseService _courseService;

        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public CourseFormRequestUpdate Input { get; set; } = new();

        public List<SelectListItem> LicenceTypes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var course = await _courseService.GetDetailAsync(id);
            if (course == null)
            {
                return RedirectToPage("./ListCourses");
            }

            // Chỉ cho phép cập nhật khi khóa học chưa bắt đầu
            var today = DateOnly.FromDateTime(DateTime.Now);
            if (course.StartDate.HasValue && today >= course.StartDate)
            {
                TempData["ErrorMessage"] = "Chỉ được cập nhật các khóa học chưa bắt đầu.";
                return RedirectToPage("./CourseDetails", new { id });
            }

            Input = new CourseFormRequestUpdate
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                LicenceTypeId = course.LicenceTypeId,
                LicenceCode = course.LicenceType.LicenceCode,
                StartDate = course.StartDate?.ToDateTime(TimeOnly.MinValue),
                EndDate = course.EndDate?.ToDateTime(TimeOnly.MinValue)
            };

            //await LoadLicenceTypesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //await LoadLicenceTypesAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra lại trước khi cập nhật
            var existing = await _courseService.GetDetailAsync(Input.CourseId);
            if (existing == null)
            {
                TempData["ErrorMessage"] = "Khóa học không tồn tại.";
                return RedirectToPage("./ListCourses");
            }
            var today = DateOnly.FromDateTime(DateTime.Now);
            if (existing.StartDate.HasValue && today >= existing.StartDate)
            {
                TempData["ErrorMessage"] = "Chỉ được cập nhật các khóa học chưa bắt đầu.";
                return RedirectToPage("./CourseDetails", new { id = Input.CourseId });
            }

            if (Input.StartDate.HasValue && Input.EndDate.HasValue && Input.StartDate > Input.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
                return Page();
            }

            try
            {
                var entity = new Course
                {
                    CourseId = Input.CourseId,
                    // CourseName không cho phép sửa trực tiếp
                    CourseName = existing.CourseName,
                    LicenceTypeId = existing.LicenceTypeId,
                    StartDate = Input.StartDate.HasValue ? DateOnly.FromDateTime(Input.StartDate.Value) : null,
                    EndDate = Input.EndDate.HasValue ? DateOnly.FromDateTime(Input.EndDate.Value) : null
                };

                await _courseService.UpdateAsync(entity);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            TempData["SuccessMessage"] = "Cập nhật khóa học thành công.";
            return RedirectToPage("./ListCourses");
        }

        //private async Task LoadLicenceTypesAsync()
        //{
        //    var types = await _courseService.GetAllLicenceTypesAsync();
        //    LicenceTypes = types.Select(t => new SelectListItem
        //    {
        //        Value = t.LicenceTypeId.ToString(),
        //        Text = t.LicenceCode
        //    }).ToList();
        //}

        public class CourseFormRequestUpdate
        {
            public int CourseId { get; set; }

            //[Required(ErrorMessage = "Tên khóa học là bắt buộc.")]
            //[StringLength(100, ErrorMessage = "Tên khóa học tối đa 100 ký tự.")]
            public string? CourseName { get; set; }

            //[Required(ErrorMessage = "Loại bằng là bắt buộc.")]
            public byte? LicenceTypeId { get; set; }
            public string? LicenceCode { get; set; }

            [Required(ErrorMessage = "Thời gian bắt đầu là bắt buộc.")]
            [DataType(DataType.Date)]
            public DateTime? StartDate { get; set; }

            [Required(ErrorMessage = "Thời gian kết thúc là bắt buộc.")]
            [DataType(DataType.Date)]
            public DateTime? EndDate { get; set; }
        }
    }
}

