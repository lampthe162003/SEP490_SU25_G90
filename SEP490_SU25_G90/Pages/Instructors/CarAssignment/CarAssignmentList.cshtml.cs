using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService;
using System.Security.Claims;

namespace SEP490_SU25_G90.Pages.Instructors.CarAssignment
{
    [Authorize(Roles = "instructor")]
    public class CarAssignmentListModel : PageModel
    {
        private readonly ICarAssignmentService _carAssignmentService;

        public CarAssignmentListModel(ICarAssignmentService carAssignmentService)
        {
            _carAssignmentService = carAssignmentService;
        }

        [BindProperty]
        public CarAssignmentSearchRequest SearchRequest { get; set; } = new();

        public List<CarAssignmentResponse> CarAssignments { get; set; } = new();
        public List<SelectListItem> CarMakes { get; set; } = new();
        public List<SelectListItem> ScheduleSlots { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDropdownData();
            await LoadCarAssignments();
            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            await LoadDropdownData();
            
            if (ModelState.IsValid)
            {
                var instructorId = GetCurrentInstructorId();
                CarAssignments = await _carAssignmentService.SearchCarAssignmentsAsync(SearchRequest, instructorId);
            }
            else
            {
                await LoadCarAssignments();
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostRentCarAsync(int carId, int slotId, string scheduleDate)
        {
            var instructorId = GetCurrentInstructorId();
            if (instructorId == null)
            {
                StatusMessage = "Không thể xác định giáo viên hiện tại.";
                return RedirectToPage();
            }

            if (!DateOnly.TryParse(scheduleDate, out var parsedDate))
            {
                StatusMessage = "Ngày không hợp lệ.";
                return RedirectToPage();
            }

            // Kiểm tra ngày không được là quá khứ
            if (parsedDate < DateOnly.FromDateTime(DateTime.Today))
            {
                StatusMessage = "Ngày mượn xe không thể là ngày trong quá khứ.";
                return RedirectToPage();
            }

            //// Kiểm tra giáo viên đã có xe trong cùng ca và ngày chưa
            //var existingAssignments = await _carAssignmentService.GetCarAssignmentsByInstructorAsync(instructorId.Value, parsedDate, slotId);
            //if (existingAssignments.Any())
            //{
            //    StatusMessage = "Bạn đã có xe được mượn trong ca học này rồi.";
            //    return RedirectToPage();
            //}

            var rentalRequest = new CarRentalRequest
            {
                CarId = carId,
                InstructorId = instructorId.Value,
                SlotId = slotId,
                ScheduleDate = parsedDate,
                CarStatus = true // Thuê thì status = 1
            };

            var success = await _carAssignmentService.RentCarAsync(rentalRequest);
            
            if (success)
            {
                StatusMessage = "Mượn xe thành công!";
            }
            else
            {
                StatusMessage = "Không thể mượn xe. Xe có thể đã được mượn bởi giáo viên khác.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostReturnCarAsync(int assignmentId)
        {
            var instructorId = GetCurrentInstructorId();
            if (instructorId == null)
            {
                StatusMessage = "Không thể xác định giáo viên hiện tại.";
                return RedirectToPage();
            }

            // Kiểm tra assignment có tồn tại và thuộc về giáo viên hiện tại không
            var assignment = await _carAssignmentService.GetCarAssignmentByIdAsync(assignmentId);
            if (assignment == null)
            {
                StatusMessage = "Không tìm thấy thông tin thuê xe.";
                return RedirectToPage();
            }

            if (assignment.InstructorId != instructorId.Value)
            {
                StatusMessage = "Bạn không có quyền trả xe này.";
                return RedirectToPage();
            }

            var success = await _carAssignmentService.ReturnCarAsync(assignmentId);
            
            if (success)
            {
                StatusMessage = "Trả xe thành công!";
            }
            else
            {
                StatusMessage = "Không thể trả xe. Vui lòng thử lại.";
            }

            return RedirectToPage();
        }

        private async Task LoadDropdownData()
        {
            CarMakes = await _carAssignmentService.GetCarMakesSelectListAsync();
            CarMakes.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả loại xe thi --" });

            ScheduleSlots = await _carAssignmentService.GetScheduleSlotsSelectListAsync();
            ScheduleSlots.Insert(0, new SelectListItem { Value = "", Text = "-- Tất cả ca học --" });
        }

        private async Task LoadCarAssignments()
        {
            CarAssignments = await _carAssignmentService.GetAllCarsWithAssignmentsAsync();
        }

        private int? GetCurrentInstructorId()
        {
            //return 2; // fake vì ko login đc
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return null;
        }
    }
} 