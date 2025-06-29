using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

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
                    (x.Learner != null && x.Learner.Cccd != null && x.Learner.Cccd.CccdNumber.Contains(searchString)) ||
                    (x.LicenceType != null && x.LicenceType.LicenceCode.Contains(searchString))
                );
            }

            var list = await (
                from la in query
                join cm in _context.ClassMembers on la.LearnerId equals cm.LearnerId into cmJoin
                from cm in cmJoin.DefaultIfEmpty()
                join cl in _context.Classes on cm.ClassId equals cl.ClassId into clJoin
                from cl in clJoin.DefaultIfEmpty()
                join instr in _context.Users on cl.InstructorId equals instr.UserId into instrJoin
                from instr in instrJoin.DefaultIfEmpty()
                select new { la, instr }
            ).ToListAsync();

            // Bổ sung logic tính trạng thái
            var licenceTypeIds = list.Select(x => x.la.LicenceTypeId).Distinct().ToList();
            var standards = await _context.TestScoreStandards
                .Where(s => licenceTypeIds.Contains(s.LicenceTypeId))
                .ToListAsync();

            var results = list.Select(x =>
            {
                var la = x.la;
                var instr = x.instr;
                var std = standards.Where(s => s.LicenceTypeId == la.LicenceTypeId).ToList();

                bool isPassed = la.TheoryScore >= std.FirstOrDefault(s => s.PartName == "Theory")?.PassScore
                             && la.SimulationScore >= std.FirstOrDefault(s => s.PartName == "Simulation")?.PassScore
                             && la.ObstacleScore >= std.FirstOrDefault(s => s.PartName == "Obstacle")?.PassScore
                             && la.PracticalScore >= std.FirstOrDefault(s => s.PartName == "Practical")?.PassScore;

                string statusName = la.LearningStatus == 3 ? "Đã huỷ" : isPassed ? "Hoàn thành" : "Đang học";

                return new LearningApplicationsResponse
                {
                    LearningId = la.LearningId,
                    LearnerId = la.LearnerId,
                    LearnerFullName = la.Learner != null ? string.Join(" ", new[] { la.Learner.LastName, la.Learner.MiddleName, la.Learner.FirstName }.Where(n => !string.IsNullOrWhiteSpace(n))) : "",
                    LearnerCccdNumber = la.Learner?.Cccd?.CccdNumber ?? "",
                    LicenceTypeId = la.LicenceTypeId,
                    LicenceTypeName = la.LicenceType?.LicenceCode ?? "",
                    InstructorId = instr?.UserId,
                    InstructorFullName = instr != null ? string.Join(" ", new[] { instr.LastName, instr.MiddleName, instr.FirstName }.Where(n => !string.IsNullOrWhiteSpace(n))) : "",
                    SubmittedAt = la.SubmittedAt,
                    LearningStatus = la.LearningStatus,
                    LearningStatusName = statusName
                };
            }).ToList();

            return results;
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

            var standards = await _context.TestScoreStandards
                .Where(s => s.LicenceTypeId == la.LicenceTypeId)
                .ToListAsync();

            bool isPassed = la.TheoryScore >= standards.FirstOrDefault(s => s.PartName == "Theory")?.PassScore
                && la.SimulationScore >= standards.FirstOrDefault(s => s.PartName == "Simulation")?.PassScore
                && la.ObstacleScore >= standards.FirstOrDefault(s => s.PartName == "Obstacle")?.PassScore
                && la.PracticalScore >= standards.FirstOrDefault(s => s.PartName == "Practical")?.PassScore;

            byte? status = isPassed ? (byte?)2 : (byte?)1;
            string? statusName = isPassed ? "Hoàn thành" : "Đang học";

            return new LearningApplicationsResponse
            {
                LearningId = la.LearningId,
                LearnerId = la.LearnerId,
                LearnerFullName = la.Learner != null ? string.Join(" ", new[] { la.Learner.LastName, la.Learner.MiddleName, la.Learner.FirstName }.Where(x => !string.IsNullOrWhiteSpace(x))) : "",
                LearnerCccdNumber = la.Learner?.Cccd?.CccdNumber ?? "",
                LearnerDob = la.Learner?.Dob?.ToDateTime(TimeOnly.MinValue),
                LearnerPhone = la.Learner?.Phone ?? "",
                LearnerEmail = la.Learner?.Email ?? "",
                LearnerCccdImageUrl = la.Learner?.Cccd != null ? (la.Learner.Cccd.ImageMt ?? "") + "|" + (la.Learner.Cccd.ImageMs ?? "") : "",
                LearnerHealthCertImageUrl = la.Learner?.HealthCertificate?.ImageUrl ?? "",
                LicenceTypeId = la.LicenceTypeId,
                LicenceTypeName = la.LicenceType?.LicenceCode ?? "",
                InstructorId = instructor?.UserId,
                InstructorFullName = instructor != null ? string.Join(" ", new[] { instructor.LastName, instructor.MiddleName, instructor.FirstName }.Where(x => !string.IsNullOrWhiteSpace(x))) : "",
                SubmittedAt = la.SubmittedAt,
                LearningStatus = status,
                LearningStatusName = statusName,
                TheoryScore = la.TheoryScore,
                SimulationScore = la.SimulationScore,
                ObstacleScore = la.ObstacleScore,
                PracticalScore = la.PracticalScore,
                TheoryPassScore = standards.FirstOrDefault(s => s.PartName == "Theory")?.PassScore,
                SimulationPassScore = standards.FirstOrDefault(s => s.PartName == "Simulation")?.PassScore,
                ObstaclePassScore = standards.FirstOrDefault(s => s.PartName == "Obstacle")?.PassScore,
                PracticalPassScore = standards.FirstOrDefault(s => s.PartName == "Practical")?.PassScore
            };
        }
        public async Task AddAsync(LearningApplication entity)
        {
            _context.LearningApplications.Add(entity);
            await _context.SaveChangesAsync();
        }

    }
}
