using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.Pages.Admins.ViewModels;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Admins.TestApplicant
{
    public class TestApplicantListModel : PageModel
    {
      /*  private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;

        public TestApplicantListModel(SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public List<TestApplicantViewModel> TestApplicants { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TestApplicants = await _context.TestApplications
                .Include(t => t.Learner) 
                    .ThenInclude(la => la.Learner) 
                .Include(t => t.Learner)
                    .ThenInclude(la => la.LicenceType)
                .Select(t => new TestApplicantViewModel
                {
                    TestId = t.TestId,
                    LearnerFullName = t.Learner.Learner.FirstName + " " +
                                      t.Learner.Learner.MiddleName + " " +
                                      t.Learner.Learner.LastName,
                    LicenceType = t.Learner.LicenceType.LicenceCode,
                    ExamDate = t.ExamDate.HasValue  
                               ? t.ExamDate.Value.ToDateTime(TimeOnly.MinValue)
                               : (DateTime?)null,
                    Score = t.Score,
                    StatusText = t.Status == null ? "Không tham gia"
                                : t.Status == true ? "Đạt" : "Trượt"
                })
                .ToListAsync();

            return Page();
        }*/
    }
}
