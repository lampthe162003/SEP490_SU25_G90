using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningMaterialRepository
{
    public interface ILearningMaterialRepository
    {
        Task<(List<LearningMaterial>, int)> GetPagedMaterialsAsync(int page, int pageSize);
    }
}
