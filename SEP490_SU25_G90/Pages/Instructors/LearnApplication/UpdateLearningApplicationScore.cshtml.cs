using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Instructors.LearnApplication
{
    [Authorize(Roles = "instructor")]
    public class UpdateLearningApplicationDetailsModel : PageModel
    {
        private readonly IInstructorService _instructorService;
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;

        public UpdateLearningApplicationDetailsModel(IInstructorService instructorService, SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context)
        {
            _instructorService = instructorService;
            _context = context;
        }

        [BindProperty]
        public LearningApplication LearningApplication { get; set; } = default!;
        public string LearnerFullName { get; set; } = "";
        public string LicenceTypeName { get; set; } = "";

        // Add the missing properties to resolve the CS0103 errors
        [BindProperty]
        public int LearningId { get; set; }

        [BindProperty]
        public int? TheoryScore { get; set; }

        [BindProperty]
        public int? SimulationScore { get; set; }

        [BindProperty]
        public int? ObstacleScore { get; set; }

        [BindProperty]
        public int? PracticalScore { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var learningapplication = await _context.LearningApplications
                .Include(l => l.Learner)
                .Include(l => l.LicenceType)
                .FirstOrDefaultAsync(m => m.LearningId == id);

            if (learningapplication == null) return NotFound();

            LearningApplication = learningapplication;
            LearnerFullName = learningapplication.Learner != null
                ? string.Join(" ", new[] { learningapplication.Learner.FirstName, learningapplication.Learner.MiddleName, learningapplication.Learner.LastName }.Where(x => !string.IsNullOrWhiteSpace(x)))
                : "Không xác định";

            LicenceTypeName = learningapplication.LicenceType?.LicenceCode ?? "Không xác định";

            // Initialize LearningId for the form
            LearningId = learningapplication.LearningId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var success = await _instructorService.UpdateLearnerScoresAsync(LearningId, TheoryScore, SimulationScore, ObstacleScore, PracticalScore);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy hồ sơ để cập nhật.");
                return Page();
            }

            TempData["SuccessMessage"] = "Cập nhật điểm thành công.";
            return RedirectToPage("./Admins/LearningApplications/List"); 
        }
    }
}
