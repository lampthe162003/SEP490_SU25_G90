using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.ScheduleSlotRepository
{
    public class ScheduleSlotRepository : IScheduleSlotRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public ScheduleSlotRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task AddSlot(ScheduleSlot scheduleSlot)
        {
            _context.ScheduleSlots.Add(scheduleSlot);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<ScheduleSlot>> GetAllSlots()
        {
            return await _context.ScheduleSlots.ToListAsync(); 
        }

        public async Task<ScheduleSlot> GetSlotById(int slotId)
        {
            return await _context.ScheduleSlots.FindAsync(slotId);
        }

        public async Task UpdateSlot(ScheduleSlot scheduleSlot)
        {
            _context.ScheduleSlots.Update(scheduleSlot);
            await _context.SaveChangesAsync();
        }


        public async Task<(List<InstructorScheduleResponse> Schedule, List<(int SlotId, string StartTime, string EndTime)> AllSlots)>
            GetWeeklyScheduleAsync(int instructorId, DateOnly startOfWeek)
        {
            var endOfWeek = startOfWeek.AddDays(6);

            // Lấy danh sách slot từ bảng ScheduleSlots
            var slots = await _context.ScheduleSlots
                .OrderBy(s => s.SlotId)
                .Select(s => new
                {
                    SlotId = s.SlotId,
                    StartTime = s.StartTime.HasValue ? s.StartTime.Value.ToString(@"HH\:mm") : string.Empty,
                    EndTime = s.EndTime.HasValue ? s.EndTime.Value.ToString(@"HH\:mm") : string.Empty
                })
                .ToListAsync();

            // Lấy lịch học từ ClassTime cho giảng viên
            var classTimes = await _context.ClassTimes
                .Include(ct => ct.Class)
                    .ThenInclude(c => c.Course)
                .Include(ct => ct.Slot)
                .Where(ct => ct.Class.InstructorId == instructorId)
                .ToListAsync();

            // Tạo schedule cho mỗi ngày trong tuần dựa trên ClassTime
            var schedule = new List<InstructorScheduleResponse>();
            
            for (int i = 0; i < 7; i++)
            {
                var currentDate = startOfWeek.AddDays(i);
                var dayOfWeek = (byte)((i + 1) % 7 + 1); // Monday = 2, Tuesday = 3, ..., Sunday = 1
                
                if (i == 6) dayOfWeek = 1; // Sunday
                else dayOfWeek = (byte)(i + 2); // Monday to Saturday
                
                var daySchedules = classTimes
                    .Where(ct => ct.Thu == dayOfWeek)
                    .Where(ct => {
                        // Kiểm tra xem ngày hiện tại có nằm trong khoảng thời gian của khóa học không
                        var course = ct.Class?.Course;
                        if (course == null) return false;
                        
                        var courseStartDate = course.StartDate;
                        var courseEndDate = course.EndDate;
                        
                        // Nếu không có ngày bắt đầu hoặc kết thúc thì hiển thị
                        if (!courseStartDate.HasValue && !courseEndDate.HasValue) return true;
                        
                        // Kiểm tra ngày hiện tại có nằm trong khoảng khóa học không
                        bool isAfterStart = !courseStartDate.HasValue || currentDate >= courseStartDate.Value;
                        bool isBeforeEnd = !courseEndDate.HasValue || currentDate <= courseEndDate.Value;
                        
                        return isAfterStart && isBeforeEnd;
                    })
                    .Select(ct => {
                        // Get attendance data for this specific date and class time
                        var attendanceData = _context.Attendances
                            .Include(a => a.Learner)
                            .Where(a => a.ClassId == ct.ClassId && 
                                       a.SessionDate == currentDate &&
                                       (a.ClassTimeId == null || a.ClassTimeId == ct.ClassTimeId))
                            .ToList();

                        // Get all class members for total count
                        var classMembers = _context.ClassMembers
                            .Include(cm => cm.Learner)
                            .Where(cm => cm.ClassId == ct.ClassId)
                            .ToList();

                        var totalStudents = classMembers.Count;
                        var presentStudents = attendanceData.Count(a => a.AttendanceStatus == true);
                        var absentStudents = attendanceData.Count(a => a.AttendanceStatus == false);
                        var attendanceRate = totalStudents > 0 ? (double)presentStudents / totalStudents * 100 : 0;

                        var studentAttendances = attendanceData.Select(a => new StudentAttendanceInfo
                        {
                            LearnerId = a.LearnerId,
                            StudentName = GetStudentName(a.Learner),
                            AttendanceStatus = a.AttendanceStatus,
                            PracticalDurationHours = a.PracticalDurationHours,
                            PracticalDistance = a.PracticalDistance,
                            Note = a.Note
                        }).ToList();

                        return new InstructorScheduleResponse
                        {
                            ScheduleDate = currentDate,
                            SlotId = ct.SlotId,
                            ClassId = ct.ClassId,
                            ClassTimeId = ct.ClassTimeId, // Include ClassTimeId for proper attendance tracking
                            ClassName = ct.Class?.ClassName ?? string.Empty,
                            StartTime = ct.Slot?.StartTime?.ToString(@"HH\:mm") ?? string.Empty,
                            EndTime = ct.Slot?.EndTime?.ToString(@"HH\:mm") ?? string.Empty,
                            DayOfWeek = dayOfWeek,
                            CourseName = ct.Class?.Course?.CourseName ?? string.Empty,
                            CourseStartDate = ct.Class?.Course?.StartDate,
                            CourseEndDate = ct.Class?.Course?.EndDate,
                            TotalStudents = totalStudents,
                            PresentStudents = presentStudents,
                            AbsentStudents = absentStudents,
                            AttendanceRate = attendanceRate,
                            StudentAttendances = studentAttendances
                        };
                    });
                
                schedule.AddRange(daySchedules);
            }

            // Convert slots sang dạng tuple để truyền ra view
            var slotList = slots
                .Select(s => (s.SlotId, s.StartTime, s.EndTime))
                .ToList();

            return (schedule, slotList);
        }

        private string GetStudentName(LearningApplication learner)
        {
            if (learner?.Learner == null) return "Unknown Student";
            
            var parts = new[]
            {
                learner.Learner.FirstName,
                learner.Learner.MiddleName,
                learner.Learner.LastName
            }.Where(part => !string.IsNullOrWhiteSpace(part));
            
            return string.Join(" ", parts);
        }
    }
}
