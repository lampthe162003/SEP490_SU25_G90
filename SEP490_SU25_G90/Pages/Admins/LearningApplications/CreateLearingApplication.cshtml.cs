using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Admins.LearningApplications
{
    [Authorize(Roles = "admin")]
    public class CreateLearingApplicationModel : PageModel
    {
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;
        private readonly ILearningApplicationService _learningApplicationService;
        private readonly IInstructorService _instructorService;
        public CreateLearingApplicationModel(
            Sep490Su25G90DbContext context,
            ILearningApplicationService learningApplicationService,
            IInstructorService instructorService)
        {
            _context = context;
            _learningApplicationService = learningApplicationService;
            _instructorService = instructorService;
        }

        
        [BindProperty]
        public LearningApplicationsResponse Input { get; set; } = new();

        [BindProperty]
        public List<LearningApplicationsResponse> LearningApplications { get; set; } = new();
        public List<InstructorListInformationResponse> Instructors { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["LearnerId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "LicenceTypeId", "LicenceCode");
            Instructors = _instructorService.GetAllInstructors().ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Fix: Explicitly convert IList to List using the constructor of List<T>
                Instructors = new List<InstructorListInformationResponse>(_instructorService.GetAllInstructors());
                return Page();
            }

            // Fix: Ensure LearningApplication is properly initialized before accessing its properties
            var learningApplication = new LearningApplication
            {
                LearnerId = LearningApplications.FirstOrDefault()?.LearnerId ?? 0,
                LicenceTypeId = LearningApplications.FirstOrDefault()?.LicenceTypeId,
                SubmittedAt = DateTime.Now // Assign registration date
            };

            await _learningApplicationService.AddAsync(learningApplication);

            return RedirectToPage("./ListLearningApplication");
        }
        
    }
}