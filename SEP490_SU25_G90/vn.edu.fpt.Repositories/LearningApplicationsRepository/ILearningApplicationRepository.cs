using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository
{
    public interface ILearningApplicationRepository
    {
        List<LearningApplication> GetAll();
        Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null);
        Task<LearningApplicationsResponse?> GetDetailAsync(int id);
        Task<IQueryable<LearningApplication>> GetAllAsync();
    }
}
