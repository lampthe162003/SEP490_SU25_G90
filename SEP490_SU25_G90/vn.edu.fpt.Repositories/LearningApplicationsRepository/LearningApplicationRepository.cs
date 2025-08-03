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
            var learningApplications = await _context.LearningApplications
                .Include(x => x.Learner).ThenInclude(l => l.Cccd)
                .Include(x => x.Learner).ThenInclude(l => l.HealthCertificate)
                .Include(x => x.LicenceType)
                .ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string lowered = searchString.Trim().ToLower();

                learningApplications = learningApplications
                    .Where(x =>
                        (x.Learner != null &&
                            string.Join(" ", new[] { x.Learner.FirstName, x.Learner.MiddleName, x.Learner.LastName }
                                .Where(n => !string.IsNullOrWhiteSpace(n)))
                            .ToLower().Contains(lowered)
                        ) ||
                        (x.Learner?.Cccd?.CccdNumber?.ToLower().Contains(lowered) ?? false) ||
                        (x.LicenceType?.LicenceCode?.ToLower().Contains(lowered) ?? false)
                    )
                    .ToList();
            }

            var learnerGroups = learningApplications
                .GroupBy(la => la.LearnerId)
                .ToList();

            var licenceTypeIds = learnerGroups
                .SelectMany(g => g.Select(la => la.LicenceTypeId))
                .Distinct()
                .ToList();

            var standards = await _context.TestScoreStandards
                .Where(s => licenceTypeIds.Contains(s.LicenceTypeId))
                .ToListAsync();

            // 🔎 Lấy giảng viên cho từng learner
            var instructorMap = await (
                from la in _context.LearningApplications
                join cm in _context.ClassMembers on la.LearnerId equals cm.LearnerId
                join c in _context.Classes on cm.ClassId equals c.ClassId
                join u in _context.Users on c.InstructorId equals u.UserId
                select new { la.LearningId, Instructor = u }
            ).ToDictionaryAsync(x => x.LearningId, x => x.Instructor);

            // ✅ Tạo danh sách kết quả
            var results = learnerGroups.Select(group =>
            {
                var mostRecent = group
                    .OrderByDescending(la => la.SubmittedAt ?? DateTime.MinValue)
                    .First();

                var std = standards.Where(s => s.LicenceTypeId == mostRecent.LicenceTypeId).ToList();

                bool isPassed = mostRecent.TheoryScore >= std.FirstOrDefault(s => s.PartName == "Theory")?.PassScore
                             && mostRecent.SimulationScore >= std.FirstOrDefault(s => s.PartName == "Simulation")?.PassScore
                             && mostRecent.ObstacleScore >= std.FirstOrDefault(s => s.PartName == "Obstacle")?.PassScore
                             && mostRecent.PracticalScore >= std.FirstOrDefault(s => s.PartName == "Practical")?.PassScore;

                string statusName = mostRecent.LearningStatus switch
                {
                    1 => "Đang học",
                    2 => "Bảo lưu",
                    3 => "Học lại",
                    4 => "Hoàn thành",
                    _ => isPassed ? "Hoàn thành" : "Chưa bắt đầu"
                };

                //  Instructor info
                //  Không cần kiểm tra HasValue nếu chắc chắn LearnerId luôn có
                User? instructor = null;
                instructorMap.TryGetValue(mostRecent.LearnerId, out instructor);
                return new LearningApplicationsResponse
                {
                    LearningId = mostRecent.LearningId,
                    LearnerId = mostRecent.LearnerId,
                    LearnerFullName = mostRecent.Learner != null
                        ? string.Join(" ", new[] { mostRecent.Learner.FirstName, mostRecent.Learner.MiddleName, mostRecent.Learner.LastName }
                            .Where(n => !string.IsNullOrWhiteSpace(n)))
                        : "",
                    LearnerCccdNumber = mostRecent.Learner?.Cccd?.CccdNumber ?? "",
                    LearnerDob = mostRecent.Learner?.Dob?.ToDateTime(TimeOnly.MinValue),
                    LearnerPhone = mostRecent.Learner?.Phone ?? "",
                    LearnerEmail = mostRecent.Learner?.Email ?? "",
                    LearnerCccdImageUrl = mostRecent.Learner?.Cccd != null
                        ? (mostRecent.Learner.Cccd.ImageMt ?? "") + "|" + (mostRecent.Learner.Cccd.ImageMs ?? "")
                        : "",
                    LearnerHealthCertImageUrl = mostRecent.Learner?.HealthCertificate?.ImageUrl ?? "",
                    LicenceTypeId = mostRecent.LicenceTypeId,
                    LicenceTypeName = mostRecent.LicenceType?.LicenceCode ?? "",
                    LearnerClasses = new List<LearnerClassInfo>(),
                    SubmittedAt = mostRecent.SubmittedAt,
                    LearningStatus = mostRecent.LearningStatus,
                    LearningStatusName = statusName,
                    InstructorId = instructor?.UserId,
                    InstructorFullName = instructor != null
                        ? string.Join(" ", new[] { instructor.FirstName, instructor.MiddleName, instructor.LastName }
                            .Where(x => !string.IsNullOrWhiteSpace(x)))
                        : ""
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

            int? theoryMaxScore = standards.FirstOrDefault(s => s.PartName == "Theory")?.MaxScore;
            int? simulationMaxScore = standards.FirstOrDefault(s => s.PartName == "Simulation")?.MaxScore;
            int? obstacleMaxScore = standards.FirstOrDefault(s => s.PartName == "Obstacle")?.MaxScore;
            int? practicalMaxScore = standards.FirstOrDefault(s => s.PartName == "Practical")?.MaxScore;

            bool isPassed = la.TheoryScore >= standards.FirstOrDefault(s => s.PartName == "Theory")?.PassScore
                && la.SimulationScore >= standards.FirstOrDefault(s => s.PartName == "Simulation")?.PassScore
                && la.ObstacleScore >= standards.FirstOrDefault(s => s.PartName == "Obstacle")?.PassScore
                && la.PracticalScore >= standards.FirstOrDefault(s => s.PartName == "Practical")?.PassScore;

            string statusName = la.LearningStatus switch
            {
                1 => "Đang học",
                2 => "Bảo lưu",
                3 => "Học lại",
                4 => "Hoàn thành",
                _ => isPassed ? "Hoàn thành" : "Chưa bắt đầu"
            };

            return new LearningApplicationsResponse
            {
                LearningId = la.LearningId,
                LearnerId = la.LearnerId,
                LearnerFullName = la.Learner != null
                    ? string.Join(" ", new[] { la.Learner.FirstName, la.Learner.MiddleName, la.Learner.LastName }
                        .Where(x => !string.IsNullOrWhiteSpace(x)))
                    : "",
                LearnerCccdNumber = la.Learner?.Cccd?.CccdNumber ?? "",
                LearnerDob = la.Learner?.Dob?.ToDateTime(TimeOnly.MinValue),
                LearnerPhone = la.Learner?.Phone ?? "",
                LearnerEmail = la.Learner?.Email ?? "",
                LearnerCccdImageUrl = la.Learner?.Cccd != null
                    ? (la.Learner.Cccd.ImageMt ?? "") + "|" + (la.Learner.Cccd.ImageMs ?? "")
                    : "",
                LearnerHealthCertImageUrl = la.Learner?.HealthCertificate?.ImageUrl ?? "",
                LicenceTypeId = la.LicenceTypeId,
                LicenceTypeName = la.LicenceType?.LicenceCode ?? "",
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
                PracticalPassScore = standards.FirstOrDefault(s => s.PartName == "Practical")?.PassScore,
                TheoryMaxScore = theoryMaxScore,
                SimulationMaxScore = simulationMaxScore,
                ObstacleMaxScore = obstacleMaxScore,
                PracticalMaxScore = practicalMaxScore,
                InstructorId = instructor?.UserId,
                InstructorFullName = instructor != null
                    ? string.Join(" ", new[] { instructor.FirstName, instructor.MiddleName, instructor.LastName }
                        .Where(x => !string.IsNullOrWhiteSpace(x)))
                    : ""
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
                var loweredSearch = searchString.ToLower();

                query = query.Where(x =>
                    (
                        x.Learner != null &&
                        (x.Learner.FirstName + " " + x.Learner.MiddleName + " " + x.Learner.LastName)
                            .ToLower()
                            .Contains(loweredSearch)
                    ) ||
                    (x.Learner != null && x.Learner.Cccd != null && x.Learner.Cccd.CccdNumber.ToLower().Contains(loweredSearch)) ||
                    (x.LicenceType != null && x.LicenceType.LicenceCode.ToLower().Contains(loweredSearch))
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

                    // 🟡 Cập nhật trạng thái mới
                    switch (la.LearningStatus)
                    {
                        case 1:
                            statusName = "Đang học";
                            badgeClass = "badge bg-primary";
                            break;
                        case 2:
                            statusName = "Bảo lưu";
                            badgeClass = "badge bg-warning text-dark";
                            break;
                        case 3:
                            statusName = "Học lại";
                            badgeClass = "badge bg-danger";
                            break;
                        case 4:
                            statusName = "Hoàn thành";
                            badgeClass = "badge bg-success";
                            break;
                        default:
                            statusName = isPassed ? "Hoàn thành" : "Chưa bắt đầu";
                            badgeClass = isPassed ? "badge bg-success" : "badge bg-secondary";
                            break;
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
                if (licenceProgresses.Any(lp => lp.LearningStatus == 1))
                {
                    overallStatus = "Đang học";
                }
                else if (licenceProgresses.All(lp => lp.LearningStatus == 4))
                {
                    overallStatus = "Hoàn thành tất cả";
                }
                else if (completedCount > 0)
                {
                    overallStatus = $"Hoàn thành {completedCount}/{totalCount}";
                }
                else if (licenceProgresses.Any(lp => lp.LearningStatus == 2))
                {
                    overallStatus = "Có bằng bảo lưu";
                }
                else if (licenceProgresses.Any(lp => lp.LearningStatus == 3))
                {
                    overallStatus = "Có bằng học lại";
                }
                else
                {
                    overallStatus = "Chưa bắt đầu";
                }

                return new LearnerSummaryResponse
                {
                    LearnerId = firstApplication.LearnerId,
                    LearnerFullName = learner != null ? string.Join(" ", new[] { learner.FirstName, learner.MiddleName, learner.LastName }.Where(n => !string.IsNullOrWhiteSpace(n))) : "",
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
            try
            {
                _context.LearningApplications.Add(entity);
                await _context.SaveChangesAsync();
                Console.WriteLine(" Hồ sơ học đã được lưu thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Lỗi khi thêm hồ sơ học: " + ex.Message);
                throw;
            }
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
                LearnerFullName = string.Join(" ", new[] { user.FirstName, user.MiddleName, user.LastName }.Where(x => !string.IsNullOrWhiteSpace(x))),
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