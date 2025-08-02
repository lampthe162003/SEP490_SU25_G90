using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService;

namespace SEP490_SU25_G90.Pages.Instructors.Car
{
    [Authorize(Roles = "instructor")]
    public class CarListModel : PageModel
    {
        
        private readonly ICarService _carService;
        private readonly ICarAssignmentService _carAssignmentService;

        public CarListModel(ICarService carService, ICarAssignmentService carAssignmentService)
        {
            
            _carService = carService;
            _carAssignmentService = carAssignmentService;
        }

       

        [BindProperty]
        public IList<vn.edu.fpt.Models.Car> Cars { get; set; } = default!;

        [BindProperty]
        public IList<CarAssignmentInformationResponse> CarAssignments { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            
            Cars = await _carService.GetAllCars();
            CarAssignments = await _carAssignmentService.GetCarAssignmentByDate(DateOnly.FromDateTime(DateTime.Now));

            return Page();
        }
    }
}
