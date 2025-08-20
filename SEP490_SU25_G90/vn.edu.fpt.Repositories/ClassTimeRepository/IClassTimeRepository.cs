using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.ClassTimeRepository
{
    public interface IClassTimeRepository
    {
        Task<List<ClassTime>> GetByClassIdAsync(int classId);
        Task AddRangeAsync(List<ClassTime> classTimes);
        Task DeleteByClassIdAsync(int classId);
        Task<bool> ExistsAsync(int classId, byte thu, int slotId);
    }
}