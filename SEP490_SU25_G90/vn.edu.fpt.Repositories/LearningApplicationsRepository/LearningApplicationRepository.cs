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

        public async Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null, int? statusFilter = null)
        {
            var query = _context.LearningApplications
                .Include(la => la.Learner)
                    .ThenInclude(l => l.Cccd)
                .Include(la => la.LicenceType)
                .Include(la => la.ClassMembers)
                    .ThenInclude(cm => cm.Class)
                        .ThenInclude(c => c.Instructor)
                .AsQueryable();

            // Bộ lọc tìm kiếm
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string lowered = searchString.Trim().ToLower();

                query = query.Where(x =>
                    (x.Learner != null &&
                     ((x.Learner.FirstName ?? "") + " " +
                      (x.Learner.MiddleName ?? "") + " " +
                      (x.Learner.LastName ?? "")
                     ).ToLower().Contains(lowered)
                    ) ||
                    (x.Learner.Cccd != null &&
                     (x.Learner.Cccd.CccdNumber ?? "").ToLower().Contains(lowered)
                    ) ||
                    (x.LicenceType != null &&
                     (x.LicenceType.LicenceCode ?? "").ToLower().Contains(lowered)
                    )
                );
            }

            // Bộ lọc trạng thái
            if (statusFilter.HasValue)
            {
                query = query.Where(x => x.LearningStatus == statusFilter.Value);
            }

            // Lấy dữ liệu và map sang DTO
            var list = await query
                .Select(la => new LearningApplicationsResponse
                {
                    LearningId = la.LearningId,
                    LearnerFullName = (la.Learner.FirstName ?? "") + " " +
                                      (la.Learner.MiddleName ?? "") + " " +
                                      (la.Learner.LastName ?? ""),
                    LearnerCccdNumber = la.Learner.Cccd != null ? la.Learner.Cccd.CccdNumber : "",
                    LicenceTypeName = la.LicenceType != null ? la.LicenceType.LicenceCode : "",
                    InstructorFullName = la.ClassMembers
                        .Select(cm => (cm.Class.Instructor.FirstName ?? "") + " " +
                                      (cm.Class.Instructor.MiddleName ?? "") + " " +
                                      (cm.Class.Instructor.LastName ?? ""))
                        .FirstOrDefault() ?? "Chưa có",
                    SubmittedAt = la.SubmittedAt,
                    LearningStatus = la.LearningStatus,
                    LearningStatusName = la.LearningStatus == 1 ? "Đang học" :
                                         la.LearningStatus == 2 ? "Bảo lưu" :
                                         la.LearningStatus == 3 ? "Học lại" :
                                         la.LearningStatus == 4 ? "Hoàn thành" :
                                         "Chưa bắt đầu"
                })
                .ToListAsync();

            return list;
        }






        public Task<IQueryable<LearningApplication>> GetAllAsync()
        {
            return Task.FromResult(_context.LearningApplications.AsQueryable());
        }

        public async Task<LearningApplicationsResponse?> GetDetailAsync(int id)
        {
            // Lấy thông tin hồ sơ học
            var la = await _context.LearningApplications
                .Include(x => x.Learner)
                .ThenInclude(l => l.Cccd)
                .Include(x => x.Learner)
                .ThenInclude(l => l.HealthCertificate)
                .Include(x => x.LicenceType)
                .FirstOrDefaultAsync(x => x.LearningId == id);

            if (la == null) return null;

            // Lấy thông tin giảng viên từ lớp học mà học viên đang tham gia
            var instructor = await (
            from cm in _context.ClassMembers
            join c in _context.Classes on cm.ClassId equals c.ClassId
            join u in _context.Users on c.InstructorId equals u.UserId
            where cm.LearnerId == la.LearnerId
            orderby cm.ClassId descending // hoặc orderby cm.Id descending nếu muốn lấy lớp mới nhất
            select u
            ).FirstOrDefaultAsync();

            // Lấy tiêu chuẩn điểm
            var standards = await _context.TestScoreStandards
                .Where(s => s.LicenceTypeId == la.LicenceTypeId)
                .ToListAsync();

            // Tổng giờ và km thực hành (chỉ tính buổi có mặt)
            var totals = await _context.Attendances
                .Where(a => a.LearnerId == la.LearnerId && a.AttendanceStatus == true)
                .GroupBy(a => a.LearnerId)
                .Select(g => new
                {
                    TotalHours = g.Sum(x => x.PracticalDurationHours) ?? 0,
                    TotalKm = g.Sum(x => x.PracticalDistance) ?? 0
                })
                .FirstOrDefaultAsync();

            double totalHours = totals?.TotalHours ?? 0;
            double totalKm = totals?.TotalKm ?? 0;

            // Giờ & km cần thiết theo loại bằng
            (int requiredHours, int requiredKm) = la.LicenceType?.LicenceCode switch
            {
                "B1" => (68, 1000),
                "B2" => (84, 1100),
                "C" => (94, 1100),
                _ => (0, 0)
            };

            // Trạng thái học
            string statusName = la.LearningStatus switch
            {
                1 => "Đang học",
                2 => "Bảo lưu",
                3 => "Học lại",
                4 => "Hoàn thành",
                _ => "Không xác định"
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
                LearnerEmail = la.Learner?.Email ?? "", // Thêm dòng này để lấy email học viên**
                LearnerPhone = la.Learner?.Phone ?? "", // Thêm dòng này để lấy số điện thoại học viên**
                LicenceTypeId = la.LicenceTypeId,
                LicenceTypeName = la.LicenceType?.LicenceCode ?? "",
                SubmittedAt = la.SubmittedAt,
                LearningStatus = la.LearningStatus,
                LearningStatusName = statusName,
                TheoryScore = la.TheoryScore,
                SimulationScore = la.SimulationScore,
                ObstacleScore = la.ObstacleScore,
                PracticalScore = la.PracticalScore,
                InstructorId = instructor?.UserId,
                InstructorFullName = instructor != null
                    ? string.Join(" ", new[] { instructor.FirstName, instructor.MiddleName, instructor.LastName }
                        .Where(x => !string.IsNullOrWhiteSpace(x)))
                    : "",
                TotalPracticalHours = totalHours,
                TotalPracticalKm = totalKm,
                RequiredPracticalHours = requiredHours,
                RequiredPracticalKm = requiredKm
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
                statusName = latestApp.LearningStatus switch
                {
                    1 => "Đang học",
                    2 => "Bảo lưu",
                    3 => "Học lại",
                    4 => "Hoàn thành",
                    _ => "Không xác định"
                };
            }

            return new LearningApplicationsResponse
            {
                LearnerId = user.UserId,
                LearnerFullName = string.Join(" ", new[] { user.FirstName, user.MiddleName, user.LastName }
                    .Where(x => !string.IsNullOrWhiteSpace(x))),
                LearnerCccdNumber = user.Cccd?.CccdNumber,
                LearnerDob = user.Dob?.ToDateTime(TimeOnly.MinValue),
                LearnerPhone = user.Phone,
                LearnerEmail = user.Email,
                LearnerCccdImageUrl = user.Cccd?.ImageMt, // hoặc ghép cả mặt trước/mặt sau nếu cần
                LearnerHealthCertImageUrl = user.HealthCertificate?.ImageUrl,
                LearningStatusName = statusName
            };
        }

        public async Task<bool> UpdateStatusAsync(int learningId, byte newStatus)
        {
            var app = await _context.LearningApplications
                .FirstOrDefaultAsync(x => x.LearningId == learningId);

            if (app == null)
                return false;

            // Cập nhật trạng thái mới
            app.LearningStatus = newStatus;

            // Nếu là Bảo lưu (2) thì xóa học viên khỏi lớp (nếu có)
            if (newStatus == 2) // Bảo lưu
            {
                var classMember = await _context.ClassMembers
                    .FirstOrDefaultAsync(cm => cm.LearnerId == app.LearnerId);

                if (classMember != null)
                {
                    _context.ClassMembers.Remove(classMember);
                }
            }
            Console.WriteLine($" [DEBUG] Cập nhật trạng thái: {learningId} -> {newStatus}");

            await _context.SaveChangesAsync();
            return true;
        }


    }
}