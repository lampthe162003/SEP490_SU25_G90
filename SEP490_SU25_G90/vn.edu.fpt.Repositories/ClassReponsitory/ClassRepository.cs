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
                .Include(c => c.Instructor)
                .Include(c => c.ClassMembers)
                .AsQueryable();

            // Tìm kiếm theo tên lớp
            if (!string.IsNullOrEmpty(searchRequest.ClassName))
            {
                query = query.Where(c => c.ClassName != null && 
                    c.ClassName.ToLower().Contains(searchRequest.ClassName.ToLower()));
            }

            // Tìm kiếm theo InstructorId
            if (searchRequest.InstructorId.HasValue)
            {
                query = query.Where(c => c.InstructorId == searchRequest.InstructorId.Value);
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
                ClassName = classDetail.ClassName,
                InstructorId = classDetail.InstructorId,
                InstructorName = classDetail.Instructor != null ?
                    $"{classDetail.Instructor.FirstName} {classDetail.Instructor.MiddleName} {classDetail.Instructor.LastName}".Trim() :
                    "Chưa phân công",
                InstructorPhone = classDetail.Instructor?.Phone,
                InstructorEmail = classDetail.Instructor?.Email,
                //Status = GetClassStatus(classDetail.StartDate, classDetail.EndDate),
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
                        LearningStatus = "Đang học", // Default status, có thể customize sau
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
    }
}
