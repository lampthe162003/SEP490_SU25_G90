using NuGet.Protocol.Core.Types;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService
{
    public class LearningApplicationService: ILearningApplicationService
    {
        private readonly ILearningApplicationRepository _learningApplicationRepository;
        public LearningApplicationService(ILearningApplicationRepository learningApplicationRepository)
        {
            _learningApplicationRepository = learningApplicationRepository;
        }
        public List<LearningApplication> GetAll()
        {
            return _learningApplicationRepository.GetAll();
        }
        public async Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null)
        {
            return await _learningApplicationRepository.GetAllAsync(searchString);
        }
    }
}
