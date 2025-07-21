using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService
{
    public interface IScheduleSlotService
    {
        public Task<IList<ScheduleSlot>> GetAllSlots();
        public Task<ScheduleSlot> GetSlotById(int slotId);
    }
}
