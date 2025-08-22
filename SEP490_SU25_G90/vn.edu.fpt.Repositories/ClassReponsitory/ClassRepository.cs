using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.ClassReponsitory
{
    /// <summary>
    /// Repository implementation cho Class entity
    /// </summary>
    public class ClassRepository : IClassRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public ClassRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy danh sách lớp học theo điều kiện tìm kiếm
        /// </summary>
        public async Task<IEnumerable<ClassListResponse>> GetClassesAsync(ClassSearchRequest searchRequest)
        {
            var query = _context.Classes
                .Include(c => c.Course)
                    .ThenInclude(cs => cs!.LicenceType)
                .Include(c => c.Instructor)
                .Include(c => c.ClassMembers)
                .AsQueryable();

            // Tìm kiếm theo tên lớp
            if (!string.IsNullOrEmpty(searchRequest.ClassName))
            {
                query = query.Where(c => c.ClassName != null &&
                    (c.ClassName.ToLower().Contains(searchRequest.ClassName.ToLower()) ||
                    (c.Instructor != null &&
                    (c.Instructor.FirstName != null && c.Instructor.FirstName.ToLower().Contains(searchRequest.ClassName.ToLower()))
                   || (c.Instructor.MiddleName != null && c.Instructor.MiddleName.ToLower().Contains(searchRequest.ClassName.ToLower()))
                   || (c.Instructor.LastName != null && c.Instructor.LastName.ToLower().Contains(searchRequest.ClassName.ToLower()))
                    )));
            }

            // Tìm kiếm theo InstructorId
            if (searchRequest.InstructorId.HasValue)
            {
                query = query.Where(c => c.InstructorId == searchRequest.InstructorId.Value);
            }

            // Lọc theo loại bằng nếu có
            if (searchRequest.LicenceTypeId.HasValue)
            {
                query = query.Where(c => c.Course != null && c.Course.LicenceTypeId == searchRequest.LicenceTypeId.Value);
            }

            var classes = await query
                .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                .Take(searchRequest.PageSize)
                .Select(c => new ClassListResponse
                {
                    ClassId = c.ClassId,
                    ClassName = c.ClassName,
                    InstructorName = c.Instructor != null ?
                        $"{c.Instructor.FirstName} {c.Instructor.MiddleName} {c.Instructor.LastName}".Trim() :
                        "Chưa phân công",
                    LicenceCode = c.Course != null && c.Course.LicenceType != null ? c.Course.LicenceType.LicenceCode : null,
                    StartDate = c.Course!.StartDate != null ? c.Course.StartDate.Value.ToDateTime(TimeOnly.MinValue) : null,
                    EndDate = c.Course!.EndDate != null ? c.Course.EndDate.Value.ToDateTime(TimeOnly.MinValue) : null,
                    Status = GetClassStatus(c.Course!.StartDate, c.Course!.EndDate),
                    TotalStudents = c.ClassMembers.Count
                })
                .ToListAsync();

            // Lọc theo trạng thái nếu có
            if (!string.IsNullOrEmpty(searchRequest.Status))
            {
                classes = classes.Where(c => c.Status == searchRequest.Status).ToList();
            }

            return classes;
        }

        /// <summary>
        /// Đếm tổng số lớp học theo điều kiện tìm kiếm
        /// </summary>
        public async Task<int> CountClassesAsync(ClassSearchRequest searchRequest)
        {
            var query = _context.Classes.AsQueryable();

            // Áp dụng các điều kiện tìm kiếm tương tự như GetClassesAsync
            if (!string.IsNullOrEmpty(searchRequest.ClassName))
            {
                query = query.Where(c => c.ClassName != null &&
                    c.ClassName.ToLower().Contains(searchRequest.ClassName.ToLower()));
            }

            if (searchRequest.InstructorId.HasValue)
            {
                query = query.Where(c => c.InstructorId == searchRequest.InstructorId.Value);
            }

            var totalClasses = await query.CountAsync();

            //// Nếu có lọc theo trạng thái, cần đếm sau khi áp dụng logic trạng thái
            //if (!string.IsNullOrEmpty(searchRequest.Status))
            //{
            //    var allClasses = await query.Select(c => new { c.StartDate, c.EndDate }).ToListAsync();
            //    var filteredCount = allClasses.Count(c => GetClassStatus(c.StartDate, c.EndDate) == searchRequest.Status);
            //    return filteredCount;
            //}

            return totalClasses;
        }

        /// <summary>
        /// Lấy tất cả loại bằng lái
        /// </summary>
        public async Task<IEnumerable<LicenceType>> GetAllLicenceTypesAsync()
        {
            return await _context.LicenceTypes.ToListAsync();
        }

        /// <summary>
        /// Lấy chi tiết lớp học theo ID
        /// </summary>
        public async Task<ClassDetailResponse?> GetClassDetailAsync(int classId)
        {
            var classDetail = await _context.Classes
                .Include(c => c.Course)
                    .ThenInclude(cs => cs!.LicenceType)
                .Include(c => c.Instructor)
                .Include(c => c.ClassMembers)
                    .ThenInclude(cm => cm.Learner)
                        .ThenInclude(la => la != null ? la.Learner : null)
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (classDetail == null)
                return null;

            var response = new ClassDetailResponse
            {
                ClassId = classDetail.ClassId,
                CourseName = classDetail.Course?.CourseName,
                ClassName = classDetail.ClassName,
                InstructorId = classDetail.InstructorId,
                InstructorName = classDetail.Instructor != null ?
                    $"{classDetail.Instructor.FirstName} {classDetail.Instructor.MiddleName} {classDetail.Instructor.LastName}".Trim() :
                    "Chưa phân công",
                InstructorPhone = classDetail.Instructor?.Phone,
                InstructorEmail = classDetail.Instructor?.Email,
                LicenceCode = classDetail.Course?.LicenceType?.LicenceCode,
                StartDate = classDetail.Course?.StartDate?.ToDateTime(TimeOnly.MinValue),
                EndDate = classDetail.Course?.EndDate?.ToDateTime(TimeOnly.MinValue),
                Status = GetClassStatus(classDetail.Course?.StartDate, classDetail.Course?.EndDate),
                TotalStudents = classDetail.ClassMembers.Count,
                Members = classDetail.ClassMembers
                    .Where(cm => cm.Learner?.Learner != null)
                    .Select(cm => new ClassMemberResponse
                    {
                        UserId = cm.Learner!.Learner!.UserId,
                        StudentCode = cm.Learner.Learner.UserId.ToString("D9"), // Generate student code from UserId
                        FullName = $"{cm.Learner.Learner.FirstName} {cm.Learner.Learner.MiddleName} {cm.Learner.Learner.LastName}".Trim(),
                        Email = cm.Learner.Learner.Email,
                        Phone = cm.Learner.Learner.Phone,
                        ProfileImageUrl = !string.IsNullOrEmpty(cm.Learner.Learner.ProfileImageUrl)
                            ? cm.Learner.Learner.ProfileImageUrl
                            : "https://cdn.vectorstock.com/i/1000v/51/87/student-avatar-user-profile-icon-vector-47025187.jpg",
                        LearningStatus = GetLearningStatusName(cm.Learner!.LearningStatus),
                        JoinDate = DateTime.Now // Có thể lấy từ CreatedDate nếu có
                    })
                    .ToList()
            };

            return response;
        }

        /// <summary>
        /// Kiểm tra lớp học có tồn tại không
        /// </summary>
        public async Task<bool> ClassExistsAsync(int classId)
        {
            return await _context.Classes.AnyAsync(c => c.ClassId == classId);
        }

        /// <summary>
        /// Lấy thông tin lớp học theo ID
        /// </summary>
        public async Task<Class?> GetClassByIdAsync(int classId)
        {
            return await _context.Classes
                .Include(c => c.Course)
                    .ThenInclude(co => co.LicenceType)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.ClassId == classId);
        }

        /// <summary>
        /// Lấy danh sách thành viên trong lớp
        /// </summary>
        public async Task<List<ClassMember>> GetClassMembersAsync(int classId)
        {
            return await _context.ClassMembers
                .Include(cm => cm.Learner)
                    .ThenInclude(la => la.Learner)
                .Where(cm => cm.ClassId == classId)
                .OrderBy(cm => cm.Learner.Learner.LastName)
                .ThenBy(cm => cm.Learner.Learner.FirstName)
                .ToListAsync();
        }

        /// <summary>
        /// Xác định trạng thái của lớp học dựa trên ngày bắt đầu và kết thúc
        /// </summary>
        private static string GetClassStatus(DateOnly? startDate, DateOnly? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
                return "Chưa xác định";

            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            if (currentDate < startDate)
                return "Chưa bắt đầu";
            else if (currentDate >= startDate && currentDate <= endDate)
                return "Đang học";
            else
                return "Đã học xong";
        }

        /// <summary>
        /// Chuyển đổi mã trạng thái học thành tên hiển thị
        /// </summary>
        private static string GetLearningStatusName(byte? status)
        {
            return status switch
            {
                1 => "Đang học",
                2 => "Bảo lưu",
                3 => "Học lại",
                4 => "Hoàn thành",
                _ => "Không xác định"
            };
        }

        /// <summary>
        /// Tạo tên lớp tự động dựa trên khóa học
        /// </summary>
        public async Task<string> GenerateClassNameAsync(int courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
            var prefix = course?.CourseName ?? ($"KH-{courseId}");

            var existingNames = await _context.Classes
                .Where(c => c.CourseId == courseId && c.ClassName != null)
                .Select(c => c.ClassName!)
                .ToListAsync();

            var maxIndex = 0;
            foreach (var name in existingNames)
            {
                var marker = "-L";
                var idx = name.LastIndexOf(marker, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0 && idx + marker.Length < name.Length)
                {
                    var numStr = name[(idx + marker.Length)..];
                    if (int.TryParse(numStr, out var n) && n > maxIndex)
                    {
                        maxIndex = n;
                    }
                }
            }

            return $"{prefix}-L{maxIndex + 1}";
        }

        /// <summary>
        /// Tạo lớp học mới
        /// </summary>
        public async Task<int> CreateClassAsync(Class newClass, List<int> selectedLearnerIds, List<string> selectedSchedules)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Classes.Add(newClass);
                await _context.SaveChangesAsync();

                // Limit selected learners by MaxStudents
                const int maxStudents = 5;
                var learnerIds = selectedLearnerIds.Take(Math.Max(0, maxStudents)).ToList();
                foreach (var learningId in learnerIds)
                {
                    _context.ClassMembers.Add(new ClassMember
                    {
                        ClassId = newClass.ClassId,
                        LearnerId = learningId
                    });

                    // Tự động cập nhật trạng thái học thành "Đang học" nếu có giảng viên
                    if (newClass.InstructorId.HasValue)
                    {
                        var learningApp = await _context.LearningApplications
                            .FirstOrDefaultAsync(la => la.LearningId == learningId);

                        if (learningApp != null && learningApp.LearningStatus != 1)
                        {
                            learningApp.LearningStatus = 1; // Đang học
                        }
                    }
                }
                await _context.SaveChangesAsync();

                // Save class schedules
                if (selectedSchedules != null && selectedSchedules.Any())
                {
                    var schedules = new List<(byte Thu, int SlotId)>();
                    foreach (var schedule in selectedSchedules)
                    {
                        var parts = schedule.Split('-');
                        if (parts.Length == 2 && byte.TryParse(parts[0], out var thu) && int.TryParse(parts[1], out var slotId))
                        {
                            schedules.Add((thu, slotId));
                        }
                    }
                    if (schedules.Any())
                    {
                        foreach (var (thu, slotId) in schedules)
                        {
                            _context.ClassTimes.Add(new ClassTime
                            {
                                ClassId = newClass.ClassId,
                                Thu = thu,
                                SlotId = slotId
                            });
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();
                return newClass.ClassId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
