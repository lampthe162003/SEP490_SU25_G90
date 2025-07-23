using Microsoft.EntityFrameworkCore;
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
    }
}
