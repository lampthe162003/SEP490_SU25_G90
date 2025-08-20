using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService;

namespace SEP490_SU25_G90.Pages.Instructors
{
    [Authorize(Roles = "instructor")]
    public class InstructorScheduleModel : PageModel
    {
        private readonly IScheduleSlotService _scheduleslotService;

        public InstructorScheduleModel(IScheduleSlotService scheduleslotService)
        {
            _scheduleslotService = scheduleslotService;
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

        [BindProperty(SupportsGet = true)]
        public int? ClassFilter { get; set; }

        public List<string> AvailableClasses { get; set; } = new();
        public List<(int ClassId, string ClassName)> AvailableClassOptions { get; set; } = new();

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
            var scheduleResult = await _scheduleslotService.GetWeeklyScheduleAsync(instructorId, StartOfWeek);

            // Extract the Schedule part of the tuple
            var allScheduleData = scheduleResult.Schedule;
            AllSlots = scheduleResult.AllSlots;

            // Get available classes for filter dropdown (before filtering)
            AvailableClasses = allScheduleData
                .Select(s => s.ClassName ?? "")
                .Distinct()
                .Where(name => !string.IsNullOrEmpty(name))
                .OrderBy(name => name)
                .ToList();

            AvailableClassOptions = allScheduleData
                .GroupBy(s => new { s.ClassId, s.ClassName })
                .Select(g => (g.Key.ClassId, g.Key.ClassName ?? ""))
                .Where(c => !string.IsNullOrEmpty(c.Item2))
                .OrderBy(c => c.Item2)
                .ToList();

            // Apply class filter if specified
            if (ClassFilter.HasValue && ClassFilter.Value > 0)
            {
                ScheduleData = allScheduleData.Where(s => s.ClassId == ClassFilter.Value).ToList();
            }
            else
            {
                ScheduleData = allScheduleData;
            }

            return Page();
        }

        /// <summary>
        /// Get URL for previous week navigation
        /// </summary>
        public string GetPreviousWeekUrl()
        {
            var previousWeek = StartOfWeek.AddDays(-7);
            var url = $"/Instructor/Schedule?Year={previousWeek.Year}&StartOfWeekInput={previousWeek:yyyy-MM-dd}";
            
            if (ClassFilter.HasValue && ClassFilter.Value > 0)
            {
                url += $"&ClassFilter={ClassFilter.Value}";
            }
            
            return url;
        }

        /// <summary>
        /// Get URL for next week navigation
        /// </summary>
        public string GetNextWeekUrl()
        {
            var nextWeek = StartOfWeek.AddDays(7);
            var url = $"/Instructor/Schedule?Year={nextWeek.Year}&StartOfWeekInput={nextWeek:yyyy-MM-dd}";
            
            if (ClassFilter.HasValue && ClassFilter.Value > 0)
            {
                url += $"&ClassFilter={ClassFilter.Value}";
            }
            
            return url;
        }

        /// <summary>
        /// Get URL for current week navigation
        /// </summary>
        public string GetCurrentWeekUrl()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var mondayOfCurrentWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            
            if (today.DayOfWeek == DayOfWeek.Sunday)
                mondayOfCurrentWeek = mondayOfCurrentWeek.AddDays(-7);
                
            var url = $"/Instructor/Schedule?Year={mondayOfCurrentWeek.Year}&StartOfWeekInput={mondayOfCurrentWeek:yyyy-MM-dd}";
            
            if (ClassFilter.HasValue && ClassFilter.Value > 0)
            {
                url += $"&ClassFilter={ClassFilter.Value}";
            }
            
            return url;
        }
    }
}
