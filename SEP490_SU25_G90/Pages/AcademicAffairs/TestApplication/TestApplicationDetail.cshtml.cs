using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestScoreStandardService;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.TestApplication
{
    public class TestApplicationDetailModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public List<TestScoreStandard> TestScoreStandards { get; set; }
        public CreatUpdateTestApplicationRequest RequestModel { get; set; } = new();
        private readonly ITestApplicationService _testApplicationService;
        private readonly ITestScoreStandardService testScoreStandardService;
        public TestApplicationDetailModel(ITestApplicationService testApplicationService,
          ITestScoreStandardService testScoreStandardService
          )
        {
            _testApplicationService = testApplicationService;
            this.testScoreStandardService = testScoreStandardService;
        }
        public async Task OnGetAsync()
        {
            RequestModel = await _testApplicationService.FindById(Id);
            TestScoreStandards = testScoreStandardService.FindByLearningApplication(RequestModel.LearningApplicationId!.Value);
        }
    }
}
