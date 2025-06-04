using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.Pages.Admins
{
    public class ListCarCourseModel : PageModel
    {
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;

        public ListCarCourseModel(SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public IList<MockTestQuestion> MockTestQuestion { get;set; } = default!;

        public async Task OnGetAsync()
        {
            MockTestQuestion = await _context.MockTestQuestions
                .Include(m => m.TestTypeNavigation).ToListAsync();
        }
    }
}
