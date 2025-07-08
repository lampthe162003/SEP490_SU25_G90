using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.LearningMaterialService
{
    public interface ILearningMaterialService
    {
        Task<(List<LearningMaterialListInformationResponse>, int)> GetPagedMaterialsAsync(int page, int pageSize);
        Task<LearningMaterialListInformationResponse?> GetMaterialByIdAsync(int id);
        Task<List<LicenceType>> GetLicenceTypesAsync();
        Task AddMaterialAsync(LearningMaterialFormRequest request);
        Task<bool> EditMaterialAsync(LearningMaterialFormRequest request);
        Task<LearningMaterialFormRequest?> GetFormByIdAsync(int id);
        Task<bool> DeleteLearningMaterialAsync(int id);
    }
}
