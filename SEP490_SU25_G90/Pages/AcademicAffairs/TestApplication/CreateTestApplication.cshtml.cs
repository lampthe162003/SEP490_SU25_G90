using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestScoreStandardService;
using System.Text.Json;

namespace SEP490_SU25_G90.Pages.Staff.TestApplication
{
    [Authorize(Roles = "staff, instructor")]
    public class CreateTestApplicationModel : PageModel
    {
        [BindProperty]
        public CreatUpdateTestApplicationRequest RequestModel { get; set; } = new();

        private readonly ITestApplicationService _testApplicationService;
        private readonly ILearningApplicationService learningApplicationService;
        public CreateTestApplicationModel(ITestApplicationService testApplicationService,
            ILearningApplicationService learningApplicationService,
            ITestScoreStandardService testScoreStandardService
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
            bool haveError = false;
            if (RequestModel.SubmitProfileDate > DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError($"" +
                       $"{nameof(RequestModel)}.{nameof(RequestModel.ExamDate)}",
                       $"Ngày nộp hồ sơ không được vượt quá ngày hôm nay"
                       );
                haveError = true;
            }

            if (RequestModel.ExamDate < RequestModel.SubmitProfileDate)
            {
                ModelState.AddModelError($"" +
                       $"{nameof(RequestModel)}.{nameof(RequestModel.ExamDate)}",
                       $"Ngày thi phải lớn hơn ngày nộp hồ sơ"
                       );
                haveError = true;
            }

            if (haveError) return Page();

            await _testApplicationService.CreateTestApplication(RequestModel);
            RequestModel = new();
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
