using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Services.LicenseTypeService;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Staff.TestApplication
{
    [Authorize(Roles = "staff, instructor")]
    public class TestApplicationListModel : PageModel
    {
        private readonly ITestApplicationService _testApplicationService;
        public TestApplicationListModel(ITestApplicationService testApplicationService,
            ILicenseTypeService licenseTypeService)
        {
            _testApplicationService = testApplicationService;
            LicenseTypes = licenseTypeService
                .GetKeyValues()
                .Select(x => new SelectListItem()
                {
                    Text = x.type,
                    Value = x.id.ToString(),
                }).ToList();
            Statuses = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Đạt",
                    Value = "1"
                },
                new SelectListItem()
                {
                    Text = "Trượt",
                    Value = "2"
                },
                new SelectListItem()
                {
                    Text = "Chưa thi",
                    Value = "0"
                }
            };
        }

        public Pagination<TestApplicationListInformationResponse> TestApplications { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? PageNumber { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? LicenseTypeId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Status { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public List<SelectListItem> LicenseTypes { get; set; }
        public List<SelectListItem> Statuses { get; set; }

        public async Task OnGetAsync()
        {
            PageNumber ??= 1;
            var searchQuery = new TestApplicationSearchRequest()
            {
                Page = PageNumber ?? 1,
                Search = Search,
                Status = Status,
                LicenseTypeId = LicenseTypeId
            };
            TestApplications = await _testApplicationService.SearchAll(searchQuery);
            if (TestApplications.TotalPage < searchQuery.Page.Value)
            {
                searchQuery.Page = TestApplications.TotalPage;
            }
        }
    }
}
