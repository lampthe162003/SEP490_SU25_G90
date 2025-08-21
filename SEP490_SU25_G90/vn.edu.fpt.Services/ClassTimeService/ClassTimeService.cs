using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.ClassTimeRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.ClassTimeService
{
    public class ClassTimeService : IClassTimeService
    {
        private readonly IClassTimeRepository _classTimeRepository;

        public ClassTimeService(IClassTimeRepository classTimeRepository)
        {
            _classTimeRepository = classTimeRepository;
        }

        public async Task<List<ClassTime>> GetClassTimesAsync(int classId)
        {
            return await _classTimeRepository.GetByClassIdAsync(classId);
        }

        public async Task SaveClassTimesAsync(int classId, List<(byte Thu, int SlotId)> schedules)
        {
            var classTimes = new List<ClassTime>();
            
            foreach (var schedule in schedules)
            {
                if (!await _classTimeRepository.ExistsAsync(classId, schedule.Thu, schedule.SlotId))
                {
                    classTimes.Add(new ClassTime
                    {
                        ClassId = classId,
                        Thu = schedule.Thu,
                        SlotId = schedule.SlotId
                    });
                }
            }

            if (classTimes.Any())
            {
                await _classTimeRepository.AddRangeAsync(classTimes);
            }
        }

        public async Task UpdateClassTimesAsync(int classId, List<(byte Thu, int SlotId)> schedules)
        {
            await _classTimeRepository.DeleteByClassIdAsync(classId);
            await SaveClassTimesAsync(classId, schedules);
        }
    }
}