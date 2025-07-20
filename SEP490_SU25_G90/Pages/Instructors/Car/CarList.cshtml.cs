using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService;

namespace SEP490_SU25_G90.Pages.Instructors.Car
{
    public class CarListModel : PageModel
    {
        
        private readonly ICarService _carService;

        public CarListModel( ICarService carService)
        {
            
            _carService = carService;
        }

       

        [BindProperty]
        public IList<vn.edu.fpt.Models.Car> Cars { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            
            Cars = await _carService.GetAllCars();

            return Page();
        }
    }
}
