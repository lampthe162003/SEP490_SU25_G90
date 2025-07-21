using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Car;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarService;

namespace SEP490_SU25_G90.Pages.Admins.Car
{
    //[Authorize(Roles = "Admin")]
    public class CarListModel : PageModel
    {
        private readonly ICarService _carService;

        public CarListModel(ICarService carService)
        {
            _carService = carService;
        }

        [BindProperty]
        public CarSearchRequest SearchRequest { get; set; } = new();

        [BindProperty]
        public CarRequest CarRequest { get; set; } = new();

        public List<CarResponse> Cars { get; set; } = new();
        public List<SelectListItem> CarMakes { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }

        public int? EditingCarId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDropdownData();
            await LoadCars();
            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            await LoadDropdownData();
            
            // Thực hiện tìm kiếm với các điều kiện được cung cấp
            Cars = await _carService.SearchCarsAsync(SearchRequest);
            
            return Page();
        }

        public async Task<IActionResult> OnPostResetSearchAsync()
        {
            // Reset tất cả điều kiện tìm kiếm
            SearchRequest = new CarSearchRequest();
            
            await LoadDropdownData();
            await LoadCars(); // Load tất cả xe
            
            return Page();
        }

        public async Task<IActionResult> OnPostCreateCarAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownData();
                await LoadCars();
                return Page();
            }

            // Check if license plate exists
            if (await _carService.IsLicensePlateExistsAsync(CarRequest.LicensePlate))
            {
                ModelState.AddModelError("CarRequest.LicensePlate", "Biển số xe này đã tồn tại.");
                await LoadDropdownData();
                await LoadCars();
                return Page();
            }

            var success = await _carService.CreateCarAsync(CarRequest);
            
            if (success)
            {
                StatusMessage = "Thêm xe thành công!";
                CarRequest = new CarRequest(); // Reset form
            }
            else
            {
                StatusMessage = "Không thể thêm xe. Vui lòng thử lại.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateCarAsync(int carId)
        {
            if (!ModelState.IsValid)
            {
                EditingCarId = carId;
                await LoadDropdownData();
                await LoadCars();
                return Page();
            }

            // Check if license plate exists (excluding current car)
            if (await _carService.IsLicensePlateExistsAsync(CarRequest.LicensePlate, carId))
            {
                ModelState.AddModelError("CarRequest.LicensePlate", "Biển số xe này đã tồn tại.");
                EditingCarId = carId;
                await LoadDropdownData();
                await LoadCars();
                return Page();
            }

            var success = await _carService.UpdateCarAsync(carId, CarRequest);
            
            if (success)
            {
                StatusMessage = "Cập nhật xe thành công!";
            }
            else
            {
                StatusMessage = "Không thể cập nhật xe. Vui lòng thử lại.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteCarAsync(int carId)
        {
            var (canDelete, message) = await _carService.CanDeleteCarAsync(carId);
            
            if (!canDelete)
            {
                StatusMessage = $"Không thể xóa xe: {message}";
                return RedirectToPage();
            }

            var success = await _carService.DeleteCarAsync(carId);
            
            if (success)
            {
                StatusMessage = "Xóa xe thành công!";
            }
            else
            {
                StatusMessage = "Không thể xóa xe. Vui lòng thử lại.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetEditCarAsync(int carId)
        {
            var carForEdit = await _carService.GetCarForEditAsync(carId);
            if (carForEdit == null)
            {
                StatusMessage = "Không tìm thấy xe cần sửa.";
                return RedirectToPage();
            }

            CarRequest = carForEdit;
            EditingCarId = carId;
            
            await LoadDropdownData();
            await LoadCars();
            return Page();
        }

        private async Task LoadDropdownData()
        {
            CarMakes = await _carService.GetCarMakesSelectListAsync();
            CarMakes.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả loại xe --" });
        }

        private async Task LoadCars()
        {
            Cars = await _carService.GetAllCarsAsync();
        }
    }
} 