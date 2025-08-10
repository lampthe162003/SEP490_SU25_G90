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

        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly? StartOfWeekInput { get; set; }

        public List<DateOnly> DatesOfWeek { get; set; } = new();
        public List<InstructorScheduleResponse> ScheduleData { get; set; } = new();
        public DateOnly StartOfWeek { get; set; }
        public DateOnly EndOfWeek { get; set; }
        public List<(int SlotId, string StartTime, string EndTime)> AllSlots { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy ID giảng viên từ JWT
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (!int.TryParse(userIdClaim, out int instructorId) || instructorId == 0)
            {
                return Unauthorized();
            }

            var today = DateTime.Today;

            // Nếu chưa có năm chọn, dùng năm hiện tại
            Year ??= today.Year;

            // Nếu chưa có tuần được chọn, dùng tuần hiện tại
            var baseDate = StartOfWeekInput?.ToDateTime(TimeOnly.MinValue) ?? today;

            // Nếu baseDate không nằm trong năm được chọn => gán lại về tuần đầu năm đó
            if (baseDate.Year != Year.Value)
            {
                baseDate = new DateTime(Year.Value, 1, 1);
            }

            // Tính ngày thứ Hai của tuần đó
            int diff = baseDate.DayOfWeek == DayOfWeek.Sunday ? -6 : DayOfWeek.Monday - baseDate.DayOfWeek;
            StartOfWeek = DateOnly.FromDateTime(baseDate.AddDays(diff));
            EndOfWeek = StartOfWeek.AddDays(6);

            // Tạo danh sách các ngày trong tuần
            DatesOfWeek = Enumerable.Range(0, 7)
                .Select(i => StartOfWeek.AddDays(i))
                .ToList();

            // Lấy lịch dạy của giảng viên (có cả StartTime và EndTime từ ScheduleSlots)
            var scheduleResult = await _instructorService.GetWeeklyScheduleAsync(instructorId, StartOfWeek);

            // Extract the Schedule part of the tuple
            ScheduleData = scheduleResult.Schedule;
            AllSlots = scheduleResult.AllSlots;

            return Page();
        }
    }
}
