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

            // Lấy lịch học trong tuần
            var schedule = await _context.ClassSchedules
                .Include(cs => cs.Class)
                .Include(cs => cs.Slot) // join bảng ScheduleSlots
                .Where(cs => cs.Class.InstructorId == instructorId &&
                             cs.ScheduleDate.HasValue &&
                             cs.ScheduleDate.Value >= startOfWeek &&
                             cs.ScheduleDate.Value <= endOfWeek)
                .Select(cs => new InstructorScheduleResponse
                {
                    ScheduleDate = cs.ScheduleDate.Value,
                    SlotId = cs.SlotId ?? 0,
                    ClassName = cs.Class.ClassName,
                    StartTime = cs.Slot != null && cs.Slot.StartTime.HasValue
                                ? cs.Slot.StartTime.Value.ToString(@"HH\:mm")
                                : string.Empty,
                    EndTime = cs.Slot != null && cs.Slot.EndTime.HasValue
                                ? cs.Slot.EndTime.Value.ToString(@"HH\:mm")
                                : string.Empty
                })
                .ToListAsync();


            // Convert slots sang dạng tuple để truyền ra view
            var slotList = slots
                .Select(s => (s.SlotId, s.StartTime, s.EndTime))
                .ToList();

            return (schedule, slotList);
        }

    }
}
