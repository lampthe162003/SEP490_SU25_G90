using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningMaterialService;

namespace SEP490_SU25_G90.Pages.Instructors.LearningMaterial
{
    [Authorize(Roles = "instructor")]
    public class LearningMaterialListModel : PageModel
    {
        private readonly ILearningMaterialService _learningMaterialService;

        public LearningMaterialListModel(ILearningMaterialService learningMaterialService)
        {
            _learningMaterialService = learningMaterialService;
        }

        public List<LearningMaterialListInformationResponse> Materials { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        public async Task OnGetAsync()
        {
            const int pageSize = 6;

            var (items, totalCount) = await _learningMaterialService.GetPagedMaterialsAsync(CurrentPage, pageSize);
            Materials = items;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
