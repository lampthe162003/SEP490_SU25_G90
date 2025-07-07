using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningMaterialRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.LearningMaterialService
{
    public class LearningMaterialService : ILearningMaterialService
    {
        private readonly ILearningMaterialRepository _iLearningMaterialRepository;
        private readonly IMapper _mapper;

        public LearningMaterialService(ILearningMaterialRepository iLearningMaterialRepository, IMapper mapper)
        {
            _iLearningMaterialRepository = iLearningMaterialRepository;
            _mapper = mapper;
        }

        public async Task<(List<LearningMaterialListInformationResponse>, int)> GetPagedMaterialsAsync(int page, int pageSize)
        {
            var (learningMaterial, totalLearningMaterial) = await _iLearningMaterialRepository.GetPagedMaterialsAsync(page, pageSize);
            var result = _mapper.Map<List<LearningMaterialListInformationResponse>>(learningMaterial);
            return (result, totalLearningMaterial);
        }
    }
}
