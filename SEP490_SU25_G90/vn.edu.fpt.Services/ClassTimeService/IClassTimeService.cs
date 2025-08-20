using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.ClassTimeService
{
    public interface IClassTimeService
    {
        Task<List<ClassTime>> GetClassTimesAsync(int classId);
        Task SaveClassTimesAsync(int classId, List<(byte Thu, int SlotId)> schedules);
        Task UpdateClassTimesAsync(int classId, List<(byte Thu, int SlotId)> schedules);
    }
}