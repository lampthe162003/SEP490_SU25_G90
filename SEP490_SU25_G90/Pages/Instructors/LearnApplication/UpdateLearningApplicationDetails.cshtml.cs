using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.Pages.Instructors.LearnApplication
{
    public class UpdateLearningApplicationDetailsModel : PageModel
    {
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;

        public UpdateLearningApplicationDetailsModel(SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public LearningApplication LearningApplication { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learningapplication =  await _context.LearningApplications.FirstOrDefaultAsync(m => m.LearningId == id);
            if (learningapplication == null)
            {
                return NotFound();
            }
            LearningApplication = learningapplication;
           ViewData["LearnerId"] = new SelectList(_context.Users, "UserId", "UserId");
           ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "LicenceTypeId", "LicenceTypeId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(LearningApplication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LearningApplicationExists(LearningApplication.LearningId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LearningApplicationExists(int id)
        {
            return _context.LearningApplications.Any(e => e.LearningId == id);
        }
    }
}
