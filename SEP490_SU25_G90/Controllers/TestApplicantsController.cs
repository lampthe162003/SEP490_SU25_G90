using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.Pages.Admins.ViewModels;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestApplicantsController : ControllerBase
    {
        private readonly Sep490Su25G90DbContext _context;

        public TestApplicantsController(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestApplicantViewModel>>> GetTestApplicants()
        {
            var testApplicants = await _context.TestApplications
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

            return Ok(testApplicants);
        }
    }
}
