using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.Pages.Commons
{
    public class LoginModel : PageModel
    {
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;

        public LoginModel(SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public String Username { get; set; } = default!;
        
        [BindProperty]
        public String Password { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
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

            try
            {
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(Username))
                {
                    return Page();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool UserExists(string username)
        {
            return _context.Users.Any(e => e.Username == username || e.Email == username);
        }
    }
}
