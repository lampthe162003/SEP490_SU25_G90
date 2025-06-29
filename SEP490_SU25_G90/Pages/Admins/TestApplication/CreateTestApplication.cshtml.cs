using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication;
using System.Text.Json;

namespace SEP490_SU25_G90.Pages.Admins.TestApplication
{
    [Authorize(Roles = "admin, instructor")]
    public class CreateTestApplicationModel : PageModel
    {
        [BindProperty]
        public CreateTestApplicationRequest RequestModel { get; set; } = new();

        private readonly ITestApplicationService _testApplicationService;
        private readonly ILearningApplicationService learningApplicationService;
        public CreateTestApplicationModel(ITestApplicationService testApplicationService,
            ILearningApplicationService learningApplicationService
            )
        {
            _testApplicationService = testApplicationService;
            this.learningApplicationService = learningApplicationService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("TestApplicationList");
        }
        public async Task<IActionResult> OnPostSearchAsync([FromBody] CreateTestApplicationSearchRequest search)
        {

            var learningApplicationInfo = await learningApplicationService
              .FindByCCCD(search.cccd, x => x.TestEligibility == true && !x.TestApplications.Any());

            return new JsonResult(learningApplicationInfo.Select(x => new
            {
                cccd = search.cccd,
                fullName = x.LearnerFullName,
                id = x.LearningId,
                licenseType = x.LicenceTypeName,
                dateOfBirth = x.LearnerDob.HasValue ? x.LearnerDob.Value.ToString("yyyy/MM/dd") : null
            }));
        }
    }
}
