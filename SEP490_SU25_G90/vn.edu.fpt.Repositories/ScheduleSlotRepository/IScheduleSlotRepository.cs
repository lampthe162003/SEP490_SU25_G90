using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.ScheduleSlotRepository
{
    public interface IScheduleSlotRepository
    {
        public Task<IList<ScheduleSlot>> GetAllSlots();
        public Task<ScheduleSlot> GetSlotById(int slotId);
        public Task AddSlot(ScheduleSlot scheduleSlot);
        public Task UpdateSlot(ScheduleSlot scheduleSlot);

        Task<(List<InstructorScheduleResponse> Schedule, List<(int SlotId, string StartTime, string EndTime)> AllSlots)>
            GetWeeklyScheduleAsync(int instructorId, DateOnly startOfWeek);
    }
}
