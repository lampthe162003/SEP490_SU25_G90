using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Admins.TestApplication
{
    public class TestApplicationListModel : PageModel
    {
        private readonly ITestApplicationService _testApplicationService;

        public TestApplicationListModel(ITestApplicationService testApplicationService)
        {
            _testApplicationService = testApplicationService;
        }

        public List<TestApplicationListInformationResponse> TestApplications { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCccd { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchName))
            {
                TestApplications = await _testApplicationService.GetByNameAsync(SearchName);
                return;
            }

            if (!string.IsNullOrEmpty(SearchCccd))
            {
                TestApplications = await _testApplicationService.GetByCccdAsync(SearchCccd);
                return;
            }

            TestApplications = await _testApplicationService.GetAllTestApplicationAsync();
        }
    }
}
