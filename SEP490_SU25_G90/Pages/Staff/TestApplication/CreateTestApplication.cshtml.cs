using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestScoreStandardService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LicenseTypeService;
using System.Text.Json;

namespace SEP490_SU25_G90.Pages.Staff.TestApplication
{
    [Authorize(Roles = "staff, instructor")]
    public class CreateTestApplicationModel : PageModel
    {
        [BindProperty]
        public BulkCreateTestApplicationsRequest BulkRequest { get; set; } = new();

        public List<(int id, string type)> LicenseTypes { get; set; } = new();

        private readonly ITestApplicationService _testApplicationService;
        private readonly ILearningApplicationService learningApplicationService;
        private readonly ILicenseTypeService licenseTypeService;
        public CreateTestApplicationModel(ITestApplicationService testApplicationService,
            ILearningApplicationService learningApplicationService,
            ITestScoreStandardService testScoreStandardService,
            ILicenseTypeService licenseTypeService
            )
        {
            _testApplicationService = testApplicationService;
            this.learningApplicationService = learningApplicationService;
            this.licenseTypeService = licenseTypeService;
        }
        public void OnGet()
        {
            LicenseTypes = licenseTypeService.GetKeyValues();
        }

        public async Task<IActionResult> OnPostEligibleAsync([FromBody] EligibleFilterRequest filter)
        {
            var eligible = await learningApplicationService.FindEligibleAsync(filter.LicenceTypeId);
            return new JsonResult(eligible.Select((x, idx) => new
            {
                index = idx + 1,
                id = x.LearningId,
                fullName = x.LearnerFullName,
                cccd = x.LearnerCccdNumber,
                licenceType = x.LicenceTypeName
            }));
        }

        public async Task<IActionResult> OnPostBulkCreateAsync([FromBody] BulkCreateTestApplicationsRequest request)
        {
            var created = await _testApplicationService.BulkCreateAsync(request);
            return new JsonResult(new { created });
        }
    }
}
