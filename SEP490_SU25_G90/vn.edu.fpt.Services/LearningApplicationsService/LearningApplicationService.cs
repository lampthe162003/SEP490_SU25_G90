using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;

public class LearningApplicationService : ILearningApplicationService
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

    public async Task<LearningApplicationsResponse?> GetDetailAsync(int id)
    {
        return await _learningApplicationRepository.GetDetailAsync(id);
    }
    public async Task AddAsync(LearningApplication entity)
    {
        await _learningApplicationRepository.AddAsync(entity);
    }
}