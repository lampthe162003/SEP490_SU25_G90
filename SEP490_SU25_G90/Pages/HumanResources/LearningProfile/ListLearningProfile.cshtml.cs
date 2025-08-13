using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;

namespace SEP490_SU25_G90.Pages.HumanResources.LearningProfile
{
    [Authorize(Roles = "human resources")]
    public class ListLearningProfileModel : PageModel
    {
        private readonly IUserService _userService;

        public ListLearningProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        public IList<LearnerUserResponse> LearningProfiles { get; set; } = new List<LearnerUserResponse>();
        
        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? SearchCccd { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        
        [TempData]
        public string? Message { get; set; }
        
        [TempData]
        public string? MessageType { get; set; }

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            PageNumber = 1; // Reset to first page when searching
            await LoadDataAsync();
            return Page();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // Get all learners
                var allProfiles = await _userService.GetAllLearnersAsync();
                
                // Apply search filters
                if (!string.IsNullOrEmpty(SearchName))
                {
                    allProfiles = allProfiles.Where(p => 
                        p.FullName.Contains(SearchName, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                
                if (!string.IsNullOrEmpty(SearchCccd))
                {
                    allProfiles = allProfiles.Where(p => 
                        p.CccdNumber.Contains(SearchCccd, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                
                // Calculate pagination
                TotalRecords = allProfiles.Count;
                TotalPages = TotalRecords > 0 ? (int)Math.Ceiling((double)TotalRecords / PageSize) : 1;
                
                // Ensure PageNumber is valid
                if (PageNumber < 1) PageNumber = 1;
                if (PageNumber > TotalPages) PageNumber = TotalPages;
                
                // Apply pagination
                LearningProfiles = allProfiles
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi tải dữ liệu: {ex.Message}";
                MessageType = "error";
                LearningProfiles = new List<LearnerUserResponse>();
            }
        }


    }
}
