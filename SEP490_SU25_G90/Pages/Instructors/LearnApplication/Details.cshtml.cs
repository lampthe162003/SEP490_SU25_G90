using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.Pages.Instructors.LearnApplication
{
    public class DetailsModel : PageModel
    {
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;

        public DetailsModel(SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public LearningApplication LearningApplication { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningapplication = await _context.LearningApplications.FirstOrDefaultAsync(m => m.LearningId == id);
            if (learningapplication == null)
            {
                return NotFound();
            }
            else
            {
                LearningApplication = learningapplication;
            }
            return Page();
        }
    }
}
