using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarService;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarStatusService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService;

namespace SEP490_SU25_G90.Pages.Instructors.Car
{
    [Authorize(Roles = "instructor")]
    public class CarListModel : PageModel
    {
        
        private readonly ICarService _carService;
        private readonly ICarAssignmentService _carAssignmentService;
        private readonly ICarStatusService _carStatusService;

        public CarListModel(ICarService carService, ICarAssignmentService carAssignmentService, ICarStatusService carStatusService)
        {
            
            _carService = carService;
            _carAssignmentService = carAssignmentService;
            _carStatusService = carStatusService;
        }

       

        [BindProperty]
        public IList<vn.edu.fpt.Models.Car> Cars { get; set; } = default!;

        [BindProperty]
        public IList<CarAssignmentInformationResponse> CarAssignments { get; set; } = default!;

        [BindProperty]
        public IList<CarAssignmentInformationResponse> RentedCars { get; set; } = default!;

        [BindProperty]
        public byte? CarStatusEdit { get; set; }

        public SelectList CarStatusOptions { get; set; } = default!;

        [BindProperty]
        public int AssignmentId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Cars = await _carService.GetAllCars();
            CarAssignments = await _carAssignmentService.GetCarAssignmentByDate(DateOnly.FromDateTime(DateTime.Now));
            RentedCars = await _carAssignmentService.GetCarAssignmentByInstructorId(int.Parse(User.FindFirst("user_id")?.Value));
            var carStatuses = await _carStatusService.GetCarAssignmentStatusAsync();
            CarStatusOptions = new SelectList(carStatuses, nameof(CarAssignmentStatus.StatusId), nameof(CarAssignmentStatus.StatusName));
            
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateCarStatusAsync()
        {
            await _carAssignmentService.UpdateCarAssignmentStatusAsync(AssignmentId, (byte)CarStatusEdit);

            return Redirect("/Instructor/Car/List");
        }
    }
}
