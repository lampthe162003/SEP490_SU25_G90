using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository
{
    public class LearningApplicationRepository: ILearningApplicationRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public LearningApplicationRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public List<LearningApplication> GetAll()
        {
            return _context.LearningApplications
                .Include(la => la.Learner)
                .Include(la => la.LicenceType)
                .Include(la => la.Instructor)
                .ToList();
        }
    }
}
