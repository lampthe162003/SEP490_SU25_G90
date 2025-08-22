using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;

namespace SEP490_SU25_G90.Pages.Instructors.LearnApplication
{
    [Authorize(Roles = "instructor")]
    public class ListLearningApplicationsModel : PageModel
    {
        private readonly IInstructorService _instructorService;
        private readonly Sep490Su25G90DbContext _context;

        public ListLearningApplicationsModel(IInstructorService instructorService, Sep490Su25G90DbContext context)
        {
            _instructorService = instructorService;
            _context = context;
        }

        public List<LearningApplicationsResponse> LearningApplications { get; set; } = new();
        public string? SearchString { get; set; }
        public int? StatusFilter { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public async Task<IActionResult> OnGetAsync(string? searchString, int? statusFilter, int pageNumber = 1)
        {
            var username = User.Identity?.Name;
            var userId = User.FindFirst("user_id")?.Value;
            if (string.IsNullOrEmpty(username) || userId == null) return Unauthorized();

            var instructor = await _context.Users.FirstOrDefaultAsync(u => u.UserId == int.Parse(userId));
            if (instructor == null) return Unauthorized();

            SearchString = searchString;
            StatusFilter = statusFilter;
            CurrentPage = pageNumber;

            var allApplications = await _instructorService.GetLearningApplicationsByInstructorAsync(instructor.UserId);

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                allApplications = allApplications.Where(app =>
                    (!string.IsNullOrWhiteSpace(app.LearnerFullName) && app.LearnerFullName.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(app.LearnerCccdNumber) && app.LearnerCccdNumber.Contains(SearchString))
                ).ToList();
            }

            if (StatusFilter.HasValue)
            {
                allApplications = allApplications
                    .Where(app => app.LearningStatus == StatusFilter)
                    .ToList();
            }


            int pageSize = 5; 
            TotalPages = (int)Math.Ceiling(allApplications.Count / (double)pageSize);
            LearningApplications = allApplications
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Page();
        }
    }
}
