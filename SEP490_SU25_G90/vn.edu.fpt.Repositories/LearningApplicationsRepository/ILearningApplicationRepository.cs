using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository
{
    public interface ILearningApplicationRepository
    {
        List<LearningApplication> GetAll();
        Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null, int? statusFilter = null);
        Task<List<LearnerSummaryResponse>> GetLearnerSummariesAsync(string? searchString = null);
        Task<LearningApplicationsResponse?> GetDetailAsync(int id);
        Task<IQueryable<LearningApplication>> GetAllAsync();
        Task AddAsync(LearningApplication entity);
        Task<LearningApplicationsResponse?> FindLearnerByCccdAsync(string cccd);
        Task<bool> UpdateStatusAsync(int learningId, byte newStatus);
        Task UpdateAsync(LearningApplication request);
        Task<LearningApplication> GetByIdAsync(int id);
        Task<List<WaitingLearnerResponse>> GetWaitingLearnersAsync();
        Task<List<WaitingLearnerResponse>> GetWaitingLearnersByCourseAsync(int courseId);

    }
}
