using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Instructors.Schedule
{
    [Authorize(Roles = "instructor")]
    public class InstructorScheduleModel : PageModel
    {
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;
        private readonly IInstructorService _instructorService;

        public InstructorScheduleModel(SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context, IInstructorService instructorService)
        {
            _context = context;
            _instructorService = instructorService;
        }
        public List<InstructorScheduleResponse> ScheduleData { get; set; } = new();
        public List<DateOnly> DatesOfWeek { get; set; } = new();

        public IList<ClassSchedule> ClassSchedule { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ClassSchedule = await _context.ClassSchedules
                .Include(c => c.Class)
                .Include(c => c.Slot).ToListAsync();
        }
    }
}
