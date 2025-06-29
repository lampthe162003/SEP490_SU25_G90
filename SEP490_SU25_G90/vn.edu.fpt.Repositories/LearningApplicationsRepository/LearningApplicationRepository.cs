using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;

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
                .ToList();
        }

        public async Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null)
        {
            var query = _context.LearningApplications
                .Include(x => x.Learner).ThenInclude(l => l.Cccd)
                .Include(x => x.Learner).ThenInclude(l => l.HealthCertificate)
                .Include(x => x.LicenceType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x =>
                    ((x.Learner != null
                        ? (x.Learner.LastName ?? "") + " " + (x.Learner.MiddleName ?? "") + " " + (x.Learner.FirstName ?? "")
                        : "")
                    .Contains(searchString)) ||
                    (x.Learner != null && x.Learner.Cccd != null && x.Learner.Cccd.CccdNumber != null && x.Learner.Cccd.CccdNumber.Contains(searchString)) ||
                    (x.LicenceType != null && x.LicenceType.LicenceCode != null && x.LicenceType.LicenceCode.Contains(searchString))
                );
            }

            var result = await (
                from la in query
                join cm in _context.ClassMembers on la.LearnerId equals cm.LearnerId into cmJoin
                from cm in cmJoin.DefaultIfEmpty()
                join cl in _context.Classes on cm.ClassId equals cl.ClassId into clJoin
                from cl in clJoin.DefaultIfEmpty()
                join instr in _context.Users on cl.InstructorId equals instr.UserId into instrJoin
                from instr in instrJoin.DefaultIfEmpty()
                select new LearningApplicationsResponse
                {
                    LearningId = la.LearningId,
                    LearnerId = la.LearnerId,
                    LearnerFullName = la.Learner != null
                        ? (la.Learner.LastName ?? "") + " " + (la.Learner.MiddleName ?? "") + " " + (la.Learner.FirstName ?? "")
                        : "",
                    LearnerCccdNumber = la.Learner != null && la.Learner.Cccd != null
                        ? la.Learner.Cccd.CccdNumber
                        : "",
                    LearnerDob = la.Learner != null && la.Learner.Dob.HasValue
                        ? la.Learner.Dob.Value.ToDateTime(TimeOnly.MinValue)
                        : (DateTime?)null,
                    LearnerPhone = la.Learner != null ? la.Learner.Phone : "",
                    LearnerEmail = la.Learner != null ? la.Learner.Email : "",
                    LearnerCccdImageUrl = la.Learner != null && la.Learner.Cccd != null
                        ? (la.Learner.Cccd.ImageMt != null ? la.Learner.Cccd.ImageMt : "") + "|" + (la.Learner.Cccd.ImageMs != null ? la.Learner.Cccd.ImageMs : "")
                        : "",
                    LearnerHealthCertImageUrl = la.Learner != null && la.Learner.HealthCertificate != null
                        ? la.Learner.HealthCertificate.ImageUrl ?? ""
                        : "",
                    LicenceTypeId = la.LicenceTypeId,
                    LicenceTypeName = la.LicenceType != null ? la.LicenceType.LicenceCode : "",
                    InstructorId = instr != null ? instr.UserId : null,
                    InstructorFullName = instr != null
                        ? (instr.LastName ?? "") + " " + (instr.MiddleName ?? "") + " " + (instr.FirstName ?? "")
                        : "",
                    SubmittedAt = la.SubmittedAt,
                    LearningStatus = la.LearningStatus,
                    LearningStatusName = la.LearningStatus == 1 ? "Đang học"
                                        : la.LearningStatus == 2 ? "Hoàn thành"
                                        : la.LearningStatus == 3 ? "Đã huỷ"
                                        : "Chưa xác định"
                }
            ).ToListAsync();

            return result;
        }

        public Task<IQueryable<LearningApplication>> GetAllAsync()
        {
            return Task.FromResult(_context.LearningApplications.AsQueryable());
        }

        public async Task<LearningApplicationsResponse?> GetDetailAsync(int id)
        {
            var la = await _context.LearningApplications
                .Include(x => x.Learner).ThenInclude(l => l.Cccd)
                .Include(x => x.Learner).ThenInclude(l => l.HealthCertificate)
                .Include(x => x.LicenceType)
                .FirstOrDefaultAsync(x => x.LearningId == id);

            if (la == null) return null;

            var instructor = await (
                from cm in _context.ClassMembers
                join cl in _context.Classes on cm.ClassId equals cl.ClassId
                join u in _context.Users on cl.InstructorId equals u.UserId
                where cm.LearnerId == la.LearnerId
                select u
            ).FirstOrDefaultAsync();

            return new LearningApplicationsResponse
            {
                LearningId = la.LearningId,
                LearnerId = la.LearnerId,
                LearnerFullName = la.Learner != null
                    ? (la.Learner.LastName ?? "") + " " + (la.Learner.MiddleName ?? "") + " " + (la.Learner.FirstName ?? "")
                    : "",
                LearnerCccdNumber = la.Learner != null && la.Learner.Cccd != null ? la.Learner.Cccd.CccdNumber : "",
                LearnerDob = la.Learner != null && la.Learner.Dob.HasValue
                    ? la.Learner.Dob.Value.ToDateTime(TimeOnly.MinValue)
                    : (DateTime?)null,
                LearnerPhone = la.Learner != null ? la.Learner.Phone : "",
                LearnerEmail = la.Learner != null ? la.Learner.Email : "",
                LearnerCccdImageUrl = la.Learner != null && la.Learner.Cccd != null
                    ? (la.Learner.Cccd.ImageMt != null ? la.Learner.Cccd.ImageMt : "") + "|" + (la.Learner.Cccd.ImageMs != null ? la.Learner.Cccd.ImageMs : "")
                    : "",
                LearnerHealthCertImageUrl = la.Learner != null && la.Learner.HealthCertificate != null
                    ? la.Learner.HealthCertificate.ImageUrl ?? ""
                    : "",
                LicenceTypeId = la.LicenceTypeId,
                LicenceTypeName = la.LicenceType != null ? la.LicenceType.LicenceCode : "",
                InstructorId = instructor?.UserId,
                InstructorFullName = instructor != null
                    ? (instructor.LastName ?? "") + " " + (instructor.MiddleName ?? "") + " " + (instructor.FirstName ?? "")
                    : "",
                SubmittedAt = la.SubmittedAt,
                LearningStatus = la.LearningStatus,
                LearningStatusName = la.LearningStatus == 1 ? "Đang học"
                                        : la.LearningStatus == 2 ? "Hoàn thành"
                                        : la.LearningStatus == 3 ? "Đã huỷ"
                                        : "Chưa xác định"
            };
        }
    }
}