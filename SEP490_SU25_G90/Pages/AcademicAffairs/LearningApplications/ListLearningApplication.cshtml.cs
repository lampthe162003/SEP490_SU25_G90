using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.LearningApplications
{
    [Authorize(Roles = "staff")]
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
            LearningApplications = await _learningApplicationService.GetAllAsync(SearchString);
            // Lấy dữ liệu cho trang hiện tại
            LearningApplications = LearningApplications
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            if (StatusFilter.HasValue)
            {
                LearningApplications = LearningApplications.Where(x => x.LearningStatus == StatusFilter.Value).ToList();
            }
            
            // Tính tổng số trang
            int totalItems = LearningApplications.Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
        }
    }
}