using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningMaterialRepository
{
    public class LearningMaterialRepository : ILearningMaterialRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public LearningMaterialRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task<(List<LearningMaterial>, int)> GetPagedMaterialsAsync(int page, int pageSize)
        {
            var totalLearningMaterial = await _context.LearningMaterials.CountAsync();

            var learningMaterial = await _context.LearningMaterials
                .Include(x => x.LicenceType)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (learningMaterial, totalLearningMaterial);
        }
    }
}
