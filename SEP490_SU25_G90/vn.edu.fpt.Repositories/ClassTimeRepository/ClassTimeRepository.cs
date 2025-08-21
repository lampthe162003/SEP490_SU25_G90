using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.ClassTimeRepository
{
    public class ClassTimeRepository : IClassTimeRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public ClassTimeRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassTime>> GetByClassIdAsync(int classId)
        {
            return await _context.ClassTimes
                .Include(ct => ct.Slot)
                .Where(ct => ct.ClassId == classId)
                .OrderBy(ct => ct.Thu)
                .ThenBy(ct => ct.Slot.StartTime)
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<ClassTime> classTimes)
        {
            await _context.ClassTimes.AddRangeAsync(classTimes);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByClassIdAsync(int classId)
        {
            var classTimes = await _context.ClassTimes
                .Where(ct => ct.ClassId == classId)
                .ToListAsync();
            
            _context.ClassTimes.RemoveRange(classTimes);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int classId, byte thu, int slotId)
        {
            return await _context.ClassTimes
                .AnyAsync(ct => ct.ClassId == classId && ct.Thu == thu && ct.SlotId == slotId);
        }
    }
}