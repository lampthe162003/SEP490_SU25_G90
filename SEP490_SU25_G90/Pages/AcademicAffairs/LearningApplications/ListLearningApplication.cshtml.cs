using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.LearningApplications
{
    [Authorize(Roles = "academic affairs")]
    public class ListLearningApplicationsModel : PageModel
    {
        private readonly ILearningApplicationService _learningApplicationService;

        public ListLearningApplicationsModel(ILearningApplicationService learningApplicationService)
        {
            _learningApplicationService = learningApplicationService;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        // Phân trang
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;

        // Bộ lọc trạng thái
        [BindProperty(SupportsGet = true)]
        public int? StatusFilter { get; set; }
        public List<LearningApplicationsResponse> LearningApplications { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allApplications = await _learningApplicationService.GetAllAsync(SearchString, StatusFilter);

            int totalItems = allApplications.Count;
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            LearningApplications = allApplications
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }


    }
}