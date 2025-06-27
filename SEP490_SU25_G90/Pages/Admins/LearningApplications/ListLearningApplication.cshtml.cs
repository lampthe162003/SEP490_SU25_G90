using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Admins.LearningApplications
{
    [Authorize(Roles = "Admin")]
    public class ListLearningApplicationsModel : PageModel
    {
        private readonly ILearningApplicationService _learningApplicationService;

        public ListLearningApplicationsModel(ILearningApplicationService learningApplicationService)
        {
            _learningApplicationService = learningApplicationService;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public List<LearningApplicationsResponse> LearningApplications { get; set; } = new();

        public async Task OnGetAsync()
        {
            LearningApplications = await _learningApplicationService.GetAllAsync(SearchString);
        }
    }
}