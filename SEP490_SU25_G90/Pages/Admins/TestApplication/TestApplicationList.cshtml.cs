using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Admins.TestApplication
{
    public class TestApplicationListModel : PageModel
    {
        private readonly ITestApplicationService _testApplicationService;

        public TestApplicationListModel(Sep490Su25G90DbContext context, IMapper mapper)
        {
            _testApplicationService = new TestApplicationService(context, mapper);
        }

        public List<TestApplicationListInformationResponse> TestApplications { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCccd { get; set; }
        public void OnGet()
        {
            if (!string.IsNullOrEmpty(SearchName))
            {
                TestApplications = _testApplicationService.GetByName(SearchName);
                return;
            }

            if (!string.IsNullOrEmpty(SearchCccd))
            {
                TestApplications = _testApplicationService.GetByCccd(SearchCccd);
                return;
            }

            TestApplications = _testApplicationService.GetAllTestApplication();
        }
    }
}
