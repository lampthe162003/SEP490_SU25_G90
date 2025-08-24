using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly Sep490Su25G90DbContext _context;

        public UpdateLearningApplicationDetailsModel(IInstructorService instructorService, Sep490Su25G90DbContext context)
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

        // Lưu điểm tối đa cho từng phần thi
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

            // Lấy chuẩn điểm tối đa từ DB
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
                TempData["ErrorMessage"] = "⚠️ Vui lòng nhập đầy đủ và đúng định dạng điểm.";
                return Page();
            }

            var success = await _instructorService.UpdateLearnerScoresAsync(
                LearningId, TheoryScore, SimulationScore, ObstacleScore, PracticalScore);

            if (!success)
            {
                // Không lấy message từ Service nữa, tự định nghĩa chung
                TempData["ErrorMessage"] = "❌ Cập nhật điểm thất bại. Điểm nhập vào không hợp lệ hoặc vượt mức cho phép.";
                return Page();
            }

            TempData["SuccessMessage"] = "✅ Cập nhật điểm thành công.";
            return RedirectToPage("/Instructors/LearnApplication/Details", new { id = LearningId });
        }
    }
}
