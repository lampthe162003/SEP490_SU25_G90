using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;

namespace SEP490_SU25_G90.Pages.Instructors
{
    [Authorize(Roles = "instructor")]
    public class InstructorScheduleModel : PageModel
    {
        private readonly IInstructorService _instructorService;

        public InstructorScheduleModel(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public List<DateOnly> DatesOfWeek { get; set; } = new();
        public List<InstructorScheduleResponse> ScheduleData { get; set; } = new();
        public DateOnly StartOfWeek { get; set; }
        public DateOnly EndOfWeek { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy ID của giảng viên từ JWT claims
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (!int.TryParse(userIdClaim, out int instructorId) || instructorId == 0)
            {
                return Unauthorized();
            }

            // Xác định ngày đầu tuần (Thứ Hai tuần hiện tại)
            var today = DateTime.Today;
            int diff = today.DayOfWeek == DayOfWeek.Sunday ? -6 : DayOfWeek.Monday - today.DayOfWeek;
            StartOfWeek = DateOnly.FromDateTime(today.AddDays(diff));
            EndOfWeek = StartOfWeek.AddDays(6);

            // Tạo danh sách ngày trong tuần (Thứ 2 - Chủ nhật)
            DatesOfWeek = Enumerable.Range(0, 7)
                .Select(i => StartOfWeek.AddDays(i))
                .ToList();

            // Lấy thời khóa biểu
            ScheduleData = await _instructorService.GetWeeklyScheduleAsync(instructorId, StartOfWeek);

            return Page();
        }
    }
}
