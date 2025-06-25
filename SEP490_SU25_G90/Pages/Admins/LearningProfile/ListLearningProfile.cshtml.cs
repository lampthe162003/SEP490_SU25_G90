using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.Pages.Admins.LearningProfile
{
    public class ListLearningProfileModel : PageModel
    {
        private readonly vn.edu.fpt.Services.LearningApplicationsService.ILearningApplicationService _learningApplicationService;

        public ListLearningProfileModel(vn.edu.fpt.Services.LearningApplicationsService.ILearningApplicationService learningApplicationService)
        {
            _learningApplicationService = learningApplicationService;
        }
        public IList<LearningApplication> LearningProfiles { get; set; } = new List<LearningApplication>();
        public void OnGet()
        {
            // Fetch all learning applications from the service
            LearningProfiles = _learningApplicationService.GetAll();
            LearningProfiles = LearningProfiles.Where(x => x.Instructor is not null).ToArray();
        }
    }
}
