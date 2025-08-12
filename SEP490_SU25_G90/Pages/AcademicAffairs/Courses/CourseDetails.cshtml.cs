using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course;
using SEP490_SU25_G90.vn.edu.fpt.Services.CourseService;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.Courses
{
    [Authorize(Roles = "academic affairs")]
    public class CourseDetailsModel : PageModel
    {
        private readonly ICourseService _courseService;

        public CourseDetailsModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public CourseDetailResponse? Course { get; set; }

        [BindProperty]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        public DateTime? EndDate { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Course = await _courseService.GetCourseDetailAsync(id);
            if (Course == null)
            {
                return RedirectToPage("./ListCourses");
            }
            StartDate = Course.StartDate;
            EndDate = Course.EndDate;
            return Page();
        }

        // Đã bỏ chức năng cập nhật trực tiếp ở trang chi tiết theo yêu cầu
    }
}

