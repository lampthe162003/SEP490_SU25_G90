using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Instructors.Car
{
    [Authorize(Roles = "instructor")]
    public class CarScheduleDetailsModel : PageModel
    {
        private readonly IScheduleSlotService _scheduleSlotService;
        private readonly ICarAssignmentService _carAssignmentService;

        public CarScheduleDetailsModel(IScheduleSlotService scheduleSlotService, ICarAssignmentService carAssignmentService)
        {
            _scheduleSlotService = scheduleSlotService;
            _carAssignmentService = carAssignmentService;
        }

        [BindProperty]
        public IList<ScheduleSlot> ScheduleSlots { get; set; } = default!;

        [BindProperty]
        public List<DateOnly> DaysOfWeek { get; set; } = default!;

        [BindProperty]
        public Dictionary<(DateOnly Date, int SlotId), CarAssignmentInformationResponse> CarAssignments { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ScheduleSlots = await _scheduleSlotService.GetAllSlots();

            var today = DateOnly.FromDateTime(DateTime.Today);
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var monday = today.AddDays(-diff);

            DaysOfWeek = Enumerable.Range(0, 5)
                .Select(i => monday.AddDays(i))
                .ToList();
            
            var assignments = await _carAssignmentService.GetAssignmentsByCarId(id);
            CarAssignments = assignments
                .Select(a => new CarAssignmentInformationResponse
                {
                    AssignmentId = a.AssignmentId,
                    CarId = a.CarId,
                    InstructorId = a.InstructorId,
                    SlotId = a.SlotId,
                    ScheduleDate = a.ScheduleDate,
                    CarStatus = a.CarStatus,
                    Instructor = a.Instructor
                })
                .ToDictionary(
                    a => ((DateOnly)a.ScheduleDate, a.SlotId),
                    a => a
                );
                    
            return Page();
        }
    }
}
