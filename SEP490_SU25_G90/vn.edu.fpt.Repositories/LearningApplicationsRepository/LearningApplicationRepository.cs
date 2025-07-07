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
            // 1. Query LearningApplications (unique per learner per licence type)
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

            var learningApplications = await query.ToListAsync();

            // 2. Group by learner to get all licence types per learner
            var learnerGroups = learningApplications
                .GroupBy(la => la.LearnerId)
                .ToList();

            // 3. Get standards for score calculation - Keep old logic for backward compatibility
            var licenceTypeIds = learnerGroups.SelectMany(g => g.Select(la => la.LicenceTypeId)).Distinct().ToList();
            var standards = await _context.TestScoreStandards
                .Where(s => licenceTypeIds.Contains(s.LicenceTypeId))
                .ToListAsync();

            // 4. Build results with most recent application per learner (backward compatibility)
            var results = learnerGroups.Select(group =>
            {
                // Get most recent application for this learner
                var mostRecent = group.OrderByDescending(la => la.SubmittedAt ?? DateTime.MinValue).First();
                var std = standards.Where(s => s.LicenceTypeId == mostRecent.LicenceTypeId).ToList();

                bool isPassed = mostRecent.TheoryScore >= std.FirstOrDefault(s => s.PartName == "Theory")?.PassScore
                             && mostRecent.SimulationScore >= std.FirstOrDefault(s => s.PartName == "Simulation")?.PassScore
                             && mostRecent.ObstacleScore >= std.FirstOrDefault(s => s.PartName == "Obstacle")?.PassScore
                             && mostRecent.PracticalScore >= std.FirstOrDefault(s => s.PartName == "Practical")?.PassScore;

                string statusName;
                if (mostRecent.LearningStatus == 3)
                {
                    statusName = "Đã huỷ";
                }
                else if (isPassed)
                {
                    statusName = "Hoàn thành";
                }
                else if (mostRecent.LearningStatus == 1)
                {
                    statusName = "Đang học";
                }
                else
                {
                    statusName = "Chưa bắt đầu";
                }

                return new LearningApplicationsResponse
                {
                    LearningId = mostRecent.LearningId,
                    LearnerId = mostRecent.LearnerId,
                    LearnerFullName = mostRecent.Learner != null ? string.Join(" ", new[] { mostRecent.Learner.LastName, mostRecent.Learner.MiddleName, mostRecent.Learner.FirstName }.Where(n => !string.IsNullOrWhiteSpace(n))) : "",
                    LearnerCccdNumber = mostRecent.Learner?.Cccd?.CccdNumber ?? "",
                    LearnerDob = mostRecent.Learner?.Dob?.ToDateTime(TimeOnly.MinValue),
                    LearnerPhone = mostRecent.Learner?.Phone ?? "",
                    LearnerEmail = mostRecent.Learner?.Email ?? "",
                    LearnerCccdImageUrl = mostRecent.Learner?.Cccd != null ? (mostRecent.Learner.Cccd.ImageMt ?? "") + "|" + (mostRecent.Learner.Cccd.ImageMs ?? "") : "",
                    LearnerHealthCertImageUrl = mostRecent.Learner?.HealthCertificate?.ImageUrl ?? "",
                    LicenceTypeId = mostRecent.LicenceTypeId,
                    LicenceTypeName = mostRecent.LicenceType?.LicenceCode ?? "",
                    LearnerClasses = new List<LearnerClassInfo>(),
                    SubmittedAt = mostRecent.SubmittedAt,
                    LearningStatus = mostRecent.LearningStatus,
                    LearningStatusName = statusName
                };
            }).ToList();

            return results;
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

            var standards = await _context.TestScoreStandards
                .Where(s => s.LicenceTypeId == la.LicenceTypeId)
                .ToListAsync();

            bool isPassed = la.TheoryScore >= standards.FirstOrDefault(s => s.PartName == "Theory")?.PassScore
                && la.SimulationScore >= standards.FirstOrDefault(s => s.PartName == "Simulation")?.PassScore
                && la.ObstacleScore >= standards.FirstOrDefault(s => s.PartName == "Obstacle")?.PassScore
                && la.PracticalScore >= standards.FirstOrDefault(s => s.PartName == "Practical")?.PassScore;

            string statusName;
            if (la.LearningStatus == 3)
            {
                statusName = "Đã huỷ";
            }
            else if (isPassed)
            {
                statusName = "Hoàn thành";
            }
            else if (la.LearningStatus == 1)
            {
                statusName = "Đang học";
            }
            else
            {
                statusName = "Chưa bắt đầu";
            }

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
                LearningStatus = la.LearningStatus,
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
        public async Task<List<LearnerSummaryResponse>> GetLearnerSummariesAsync(string? searchString = null)
        {
            // 1. Query LearningApplications với search
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

            var learningApplications = await query.ToListAsync();

            // 2. Group by learner ID và xử lý từng học viên
            var learnerGroups = learningApplications
                .GroupBy(la => la.LearnerId)
                .ToList();

            // 3. Get standards for score calculation
            var allLicenceTypeIds = learningApplications.Select(x => x.LicenceTypeId).Distinct().ToList();
            var standards = await _context.TestScoreStandards
                .Where(s => allLicenceTypeIds.Contains(s.LicenceTypeId))
                .ToListAsync();

            // 4. Build LearnerSummaryResponse for each learner
            var results = learnerGroups.Select(group =>
            {
                var firstApplication = group.First(); // Lấy thông tin cơ bản từ application đầu tiên
                var learner = firstApplication.Learner;

                // Xử lý từng loại bằng của học viên này
                var licenceProgresses = group.Select(la =>
                {
                    var std = standards.Where(s => s.LicenceTypeId == la.LicenceTypeId).ToList();

                    bool isPassed = la.TheoryScore >= std.FirstOrDefault(s => s.PartName == "Theory")?.PassScore
                                 && la.SimulationScore >= std.FirstOrDefault(s => s.PartName == "Simulation")?.PassScore
                                 && la.ObstacleScore >= std.FirstOrDefault(s => s.PartName == "Obstacle")?.PassScore
                                 && la.PracticalScore >= std.FirstOrDefault(s => s.PartName == "Practical")?.PassScore;

                    string statusName;
                    string badgeClass;
                    if (la.LearningStatus == 3)
                    {
                        statusName = "Đã huỷ";
                        badgeClass = "badge bg-danger";
                    }
                    else if (isPassed)
                    {
                        statusName = "Hoàn thành";
                        badgeClass = "badge bg-success";
                    }
                    else if (la.LearningStatus == 1)
                    {
                        statusName = "Đang học";
                        badgeClass = "badge bg-primary";
                    }
                    else
                    {
                        statusName = "Chưa bắt đầu";
                        badgeClass = "badge bg-warning text-dark";
                    }

                    return new LicenceProgress
                    {
                        LearningId = la.LearningId,
                        LicenceTypeId = la.LicenceTypeId,
                        LicenceTypeName = la.LicenceType?.LicenceCode ?? "",
                        SubmittedAt = la.SubmittedAt,
                        LearningStatus = la.LearningStatus,
                        LearningStatusName = statusName,
                        TheoryScore = la.TheoryScore,
                        SimulationScore = la.SimulationScore,
                        ObstacleScore = la.ObstacleScore,
                        PracticalScore = la.PracticalScore,
                        IsCompleted = isPassed,
                        StatusBadgeClass = badgeClass
                    };
                }).OrderByDescending(lp => lp.SubmittedAt).ToList();

                // Tính overall status dựa trên tất cả licences
                var completedCount = licenceProgresses.Count(lp => lp.IsCompleted);
                var totalCount = licenceProgresses.Count;
                
                string overallStatus;
                if (licenceProgresses.Any(lp => lp.LearningStatusName == "Đang học"))
                {
                    overallStatus = "Đang học";
                }
                else if (completedCount == totalCount && totalCount > 0)
                {
                    overallStatus = "Hoàn thành tất cả";
                }
                else if (completedCount > 0)
                {
                    overallStatus = $"Hoàn thành {completedCount}/{totalCount}";
                }
                else if (licenceProgresses.Any(lp => lp.LearningStatusName == "Đã huỷ"))
                {
                    overallStatus = "Có bằng bị huỷ";
                }
                else
                {
                    overallStatus = "Chưa bắt đầu";
                }

                return new LearnerSummaryResponse
                {
                    LearnerId = firstApplication.LearnerId,
                    LearnerFullName = learner != null ? string.Join(" ", new[] { learner.LastName, learner.MiddleName, learner.FirstName }.Where(n => !string.IsNullOrWhiteSpace(n))) : "",
                    LearnerCccdNumber = learner?.Cccd?.CccdNumber ?? "",
                    LearnerDob = learner?.Dob?.ToDateTime(TimeOnly.MinValue),
                    LearnerPhone = learner?.Phone ?? "",
                    LearnerEmail = learner?.Email ?? "",
                    LearnerCccdImageUrl = learner?.Cccd != null ? (learner.Cccd.ImageMt ?? "") + "|" + (learner.Cccd.ImageMs ?? "") : "",
                    LearnerHealthCertImageUrl = learner?.HealthCertificate?.ImageUrl ?? "",
                    LicenceProgresses = licenceProgresses,
                    LatestSubmittedAt = licenceProgresses.Any() ? licenceProgresses.Max(lp => lp.SubmittedAt) : null,
                    OverallStatus = overallStatus,
                    CompletedLicences = completedCount,
                    TotalLicences = totalCount
                };
            }).ToList();

            return results;
        }

        public async Task AddAsync(LearningApplication entity)
        {
            _context.LearningApplications.Add(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<LearningApplicationsResponse?> FindLearnerByCccdAsync(string cccd)
        {
            var user = await _context.Users
                .Include(u => u.Cccd)
                .Include(u => u.HealthCertificate)
                .FirstOrDefaultAsync(u => u.Cccd != null && u.Cccd.CccdNumber == cccd);

            if (user == null)
                return null;

            // Kiểm tra trạng thái hồ sơ học gần nhất (nếu có)
            var latestApp = await _context.LearningApplications
                .Where(la => la.LearnerId == user.UserId)
                .OrderByDescending(la => la.SubmittedAt)
                .FirstOrDefaultAsync();

            string? statusName = null;
            if (latestApp != null)
            {
                if (latestApp.LearningStatus == 3)
                    statusName = "Đã huỷ";
                else if (latestApp.LearningStatus == 2)
                    statusName = "Hoàn thành";
                else if (latestApp.LearningStatus == 1)
                    statusName = "Đang học";
            }

            return new LearningApplicationsResponse
            {
                LearnerId = user.UserId,
                LearnerFullName = string.Join(" ", new[] { user.LastName, user.MiddleName, user.FirstName }.Where(x => !string.IsNullOrWhiteSpace(x))),
                LearnerCccdNumber = user.Cccd?.CccdNumber,
                LearnerDob = user.Dob?.ToDateTime(TimeOnly.MinValue),
                LearnerPhone = user.Phone,
                LearnerEmail = user.Email,
                LearnerCccdImageUrl = user.Cccd?.ImageMt, // hoặc ghép cả mặt trước/mặt sau nếu cần
                LearnerHealthCertImageUrl = user.HealthCertificate?.ImageUrl,
                LearningStatusName = statusName
            };
        }
    }
}
