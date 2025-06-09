using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplicantion;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Admins.TestApplicantion
{
    public class TestApplicantionListModel : PageModel
    {
        private readonly ITestApplicantionService _testApplicantionService;

        public TestApplicantionListModel(Sep490Su25G90DbContext context, IMapper mapper)
        {
            _testApplicantionService = new TestApplicantionService(context, mapper);
        }

        public List<TestApplicantionListInformationResponse> TestApplicants { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchCccd { get; set; }
        public void OnGet()
        {
            if (!string.IsNullOrEmpty(SearchName))
            {
                TestApplicants = _testApplicantionService.GetByName(SearchName);
                return;
            }

            if (!string.IsNullOrEmpty(SearchCccd))
            {
                TestApplicants = _testApplicantionService.GetByCccd(SearchCccd);
                return;
            }

            TestApplicants = _testApplicantionService.GetAll();
        }
    }
}
