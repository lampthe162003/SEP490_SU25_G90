using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.LearningMaterialService
{
    public interface ILearningMaterialService
    {
        Task<(List<LearningMaterialListInformationResponse>, int)> GetPagedMaterialsAsync(int page, int pageSize);
    }
}
