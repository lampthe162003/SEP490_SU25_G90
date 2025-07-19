using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;

namespace SEP490_SU25_G90.Pages.Admins.LearningApplications
{
    [Authorize(Roles = "admin")]
    [Authorize(Roles = "instructor")]
    public class LearningApplicationDetailModel : PageModel
    {
        private readonly ILearningApplicationService _learningApplicationService;

        public LearningApplicationDetailModel(ILearningApplicationService learningApplicationService)
        {
            _learningApplicationService = learningApplicationService;
        }

        public LearningApplicationsResponse? LearningApplication { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detail = await _learningApplicationService.GetDetailAsync(id.Value);
            if (detail == null)
            {
                return NotFound();
            }

            LearningApplication = detail;
            return Page();
        }
    }
}