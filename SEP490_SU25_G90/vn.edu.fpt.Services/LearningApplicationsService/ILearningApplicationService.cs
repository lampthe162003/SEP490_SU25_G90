using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService
{
    public interface ILearningApplicationService
    {
        public List<LearningApplication> GetAll();
        Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null);
    }
}

