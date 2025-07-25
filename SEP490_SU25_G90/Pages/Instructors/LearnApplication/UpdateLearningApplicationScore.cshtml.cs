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

        [BindProperty] public int LearningId { get; set; }
        [BindProperty] public int? TheoryScore { get; set; }
        [BindProperty] public int? SimulationScore { get; set; }
        [BindProperty] public int? ObstacleScore { get; set; }
        [BindProperty] public int? PracticalScore { get; set; }

        public string LearnerName { get; set; } = "";
        public string LicenceCode { get; set; } = "";
        public DateTime? SubmittedAt { get; set; }

        // Dùng để bind điểm tối đa theo từng phần
        public Dictionary<string, int> MaxScores { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var app = await _context.LearningApplications
                .Include(x => x.Learner)
                .Include(x => x.LicenceType)
                .FirstOrDefaultAsync(x => x.LearningId == id);

            if (app == null) return NotFound();

            LearningId = app.LearningId;
            TheoryScore = app.TheoryScore;
            SimulationScore = app.SimulationScore;
            ObstacleScore = app.ObstacleScore;
            PracticalScore = app.PracticalScore;

            LearnerName = $"{app.Learner?.FirstName} {app.Learner?.MiddleName} {app.Learner?.LastName}".Trim();
            LicenceCode = app.LicenceType?.LicenceCode ?? "";
            SubmittedAt = app.SubmittedAt;

            // Lấy điểm tối đa từ bảng TestScoreStandards
            var standards = await _context.TestScoreStandards
                .Where(s => s.LicenceTypeId == app.LicenceTypeId)
                .ToListAsync();

            foreach (var std in standards)
            {
                MaxScores[std.PartName] = (int)std.MaxScore;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Dữ liệu không hợp lệ.");
                return Page();
            }

            var result = await _instructorService.UpdateLearnerScoresAsync(LearningId, TheoryScore, SimulationScore, ObstacleScore, PracticalScore);

            if (!result)
            {
                TempData["ErrorMessage"] = "Cập nhật điểm thất bại. Điểm có thể vượt mức cho phép.";
                return Page();
            }

            TempData["SuccessMessage"] = "Cập nhật điểm thành công.";
            return RedirectToPage("/Instructors/LearnApplication/List");
        }
    }
}
