using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.Pages.Admins.LearningProfile
{
    //[Authorize(Roles = "admin")]
    public class ListLearningProfileModel : PageModel
    {
        private readonly vn.edu.fpt.Services.InstructorService.IInstructorService _instructorService;

        public ListLearningProfileModel(vn.edu.fpt.Services.InstructorService.IInstructorService instructorService)
        {
            _instructorService = instructorService;
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
                // Build search string from name and cccd
                string? searchString = null;
                if (!string.IsNullOrEmpty(SearchName) || !string.IsNullOrEmpty(SearchCccd))
                {
                    var searchTerms = new List<string>();
                    if (!string.IsNullOrEmpty(SearchName)) searchTerms.Add(SearchName);
                    if (!string.IsNullOrEmpty(SearchCccd)) searchTerms.Add(SearchCccd);
                    searchString = string.Join(" ", searchTerms);
                }

                // Get all learners
                var allProfiles = await _instructorService.GetAllLearnersAsync(searchString);
                
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
