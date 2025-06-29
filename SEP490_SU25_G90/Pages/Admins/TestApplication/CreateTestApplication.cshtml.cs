using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
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
        public CreateTestApplicationModel(ITestApplicationService testApplicationService)
        {
            _testApplicationService = testApplicationService;
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

            // TODO: Lấy dữ liệu từ DB theo CCCD
            var results = new List<object>
            {
            new {
                text = $"Đỗ Đức Tuấn - B1",
                cccd = search.cccd,
                fullname = "Đỗ Đức Tuấn",
                birthday = "31/10/2001",
                licenseType = "B1"
            }
            };

            return new JsonResult(results);
        }
    }
}
