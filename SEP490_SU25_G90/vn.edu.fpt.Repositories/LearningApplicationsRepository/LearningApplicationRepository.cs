using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository
{
    public class LearningApplicationRepository : ILearningApplicationRepository
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
        public async Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null)
        {
            var query = _context.LearningApplications
                .Include(x => x.Learner).ThenInclude(l => l.Cccd)
                .Include(x => x.LicenceType)
                .Include(x => x.Instructor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                    ((x.Learner.LastName + " " + x.Learner.MiddleName + " " + x.Learner.FirstName).Contains(searchString)) ||
                    (x.Learner.Cccd.CccdNumber ?? "").Contains(searchString) ||
                    (x.LicenceType.LicenceCode ?? "").Contains(searchString) // Fixed: Replaced LicenceTypeName with LicenceCode
                );
            }

            return await query
                .Select(x => new LearningApplicationsResponse
                {
                    LearningId = x.LearningId,
                    LearnerId = x.LearnerId,
                    LearnerFullName = x.Learner.LastName + " " + x.Learner.MiddleName + " " + x.Learner.FirstName,
                    LearnerCccdNumber = x.Learner.Cccd.CccdNumber,
                    LicenceTypeId = x.LicenceTypeId,
                    LicenceTypeName = x.LicenceType.LicenceCode, // Fixed: Replaced LicenceTypeName with LicenceCode
                    InstructorId = x.InstructorId,
                    InstructorFullName = x.Instructor.LastName + " " + x.Instructor.MiddleName + " " + x.Instructor.FirstName,
                    SubmittedAt = x.SubmittedAt,
                    LearningStatus = x.LearningStatus,
                    LearningStatusName = x.LearningStatus == 1 ? "Đang học" :
                                        x.LearningStatus == 2 ? "Hoàn thành" :
                                        x.LearningStatus == 3 ? "Đã huỷ" : "Chưa xác định"
                })
                .ToListAsync();
        }
    }
}
