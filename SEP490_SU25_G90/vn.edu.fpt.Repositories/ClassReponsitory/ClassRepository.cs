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
                .Include(c => c.LicenceType)
                .Include(c => c.ClassMembers)
                .AsQueryable();

            // Tìm kiếm theo tên lớp
            if (!string.IsNullOrEmpty(searchRequest.ClassName))
            {
                query = query.Where(c => c.ClassName != null && 
                    c.ClassName.ToLower().Contains(searchRequest.ClassName.ToLower()));
            }

            // Tìm kiếm theo loại bằng
            if (searchRequest.LicenceTypeId.HasValue)
            {
                query = query.Where(c => c.LicenceTypeId == searchRequest.LicenceTypeId.Value);
            }

            // Tìm kiếm theo ngày bắt đầu
            if (searchRequest.StartDate.HasValue)
            {
                var startDateOnly = DateOnly.FromDateTime(searchRequest.StartDate.Value);
                query = query.Where(c => c.StartDate >= startDateOnly);
            }

            // Tìm kiếm theo ngày kết thúc
            if (searchRequest.EndDate.HasValue)
            {
                var endDateOnly = DateOnly.FromDateTime(searchRequest.EndDate.Value);
                query = query.Where(c => c.EndDate <= endDateOnly);
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
                    LicenceCode = c.LicenceType != null ? c.LicenceType.LicenceCode : "",
                    StartDate = c.StartDate.HasValue ? c.StartDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    EndDate = c.EndDate.HasValue ? c.EndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    Status = GetClassStatus(c.StartDate, c.EndDate),
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

            if (searchRequest.LicenceTypeId.HasValue)
            {
                query = query.Where(c => c.LicenceTypeId == searchRequest.LicenceTypeId.Value);
            }

            if (searchRequest.StartDate.HasValue)
            {
                var startDateOnly = DateOnly.FromDateTime(searchRequest.StartDate.Value);
                query = query.Where(c => c.StartDate >= startDateOnly);
            }

            if (searchRequest.EndDate.HasValue)
            {
                var endDateOnly = DateOnly.FromDateTime(searchRequest.EndDate.Value);
                query = query.Where(c => c.EndDate <= endDateOnly);
            }

            var totalClasses = await query.CountAsync();

            // Nếu có lọc theo trạng thái, cần đếm sau khi áp dụng logic trạng thái
            if (!string.IsNullOrEmpty(searchRequest.Status))
            {
                var allClasses = await query.Select(c => new { c.StartDate, c.EndDate }).ToListAsync();
                var filteredCount = allClasses.Count(c => GetClassStatus(c.StartDate, c.EndDate) == searchRequest.Status);
                return filteredCount;
            }

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
