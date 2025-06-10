using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.Pages.Admins.CarCourse
{
    public class ListModel : PageModel
    {
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;

        public ListModel(SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public IList<Course> Course { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Courses
                .Include(c => c.LicenceType)
                .Include(c => c.Videos)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                query = query.Where(c =>
                    (c.Title != null && c.Title.Contains(SearchString)) ||
                    (c.Description != null && c.Description.Contains(SearchString)));
            }

            Course = await query.OrderBy(c => c.Title).ToListAsync();
        }




    }
}
