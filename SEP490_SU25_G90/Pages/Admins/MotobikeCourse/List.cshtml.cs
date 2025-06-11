using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.Course;

namespace SEP490_SU25_G90.Pages.Admins.MotobikeCourse
{
    public class ListModel : PageModel
    {
        private readonly IMotobikeCourseService _motobikeCourseService;

        public ListModel(IMotobikeCourseService motobikeCourseService)
        {
            _motobikeCourseService = motobikeCourseService;
        }

        public IList<CourseInformationResponse> Course { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            Course = await _motobikeCourseService.GetAllMotobikeCourseAsync(SearchString);
        }
    }
}
