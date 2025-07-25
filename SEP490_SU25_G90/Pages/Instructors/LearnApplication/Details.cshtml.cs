using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Instructors.LearnApplication
{
    [Authorize(Roles = "instructor")]
    public class DetailModel : PageModel
    {
        private readonly IInstructorService _instructorService;
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;
        public DetailModel(IInstructorService instructorService, SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context)
        {
            _instructorService = instructorService;
            _context = context;
        }

        public LearningApplicationsResponse? Detail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Detail = await _instructorService.GetLearningApplicationDetailAsync(id);
            if (Detail == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
