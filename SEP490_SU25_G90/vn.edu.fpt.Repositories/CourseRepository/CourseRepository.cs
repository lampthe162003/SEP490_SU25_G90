using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CourseRepository
{
    /// <summary>
    /// Repository triển khai cho Course
    /// </summary>
    public class CourseRepository : ICourseRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public CourseRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseListResponse>> GetCoursesAsync(CourseSearchRequest searchRequest)
        {
            var query = _context.Courses
                .Include(c => c.Classes)
                    .ThenInclude(cl => cl.ClassMembers)
                .Include(c => c.LicenceType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchRequest.CourseName))
            {
                var name = searchRequest.CourseName.Trim().ToLower();
                query = query.Where(c => c.CourseName != null && c.CourseName.ToLower().Contains(name));
            }

            if (searchRequest.LicenceTypeId.HasValue)
            {
                query = query.Where(c => c.LicenceTypeId == searchRequest.LicenceTypeId);
            }

            if (searchRequest.StartDate.HasValue)
            {
                var start = DateOnly.FromDateTime(searchRequest.StartDate.Value.Date);
                query = query.Where(c => !c.StartDate.HasValue || c.StartDate >= start);
            }

            if (searchRequest.EndDate.HasValue)
            {
                var end = DateOnly.FromDateTime(searchRequest.EndDate.Value.Date);
                query = query.Where(c => !c.EndDate.HasValue || c.EndDate <= end);
            }

            var projected = query
                .OrderByDescending(c => c.CourseId)
                .Skip((searchRequest.PageNumber - 1) * searchRequest.PageSize)
                .Take(searchRequest.PageSize)
                .Select(c => new CourseListResponse
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    LicenceCode = c.LicenceType != null ? c.LicenceType.LicenceCode : null,
                    StartDate = c.StartDate.HasValue ? c.StartDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    EndDate = c.EndDate.HasValue ? c.EndDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    TotalClasses = c.Classes.Count,
                    TotalStudents = c.Classes.SelectMany(cl => cl.ClassMembers).Count()
                });

            var list = await projected.ToListAsync();

            // Tính trạng thái sau khi lấy ra (dựa vào ngày)
            foreach (var item in list)
            {
                item.Status = GetCourseStatus(
                    item.StartDate.HasValue ? DateOnly.FromDateTime(item.StartDate.Value) : (DateOnly?)null,
                    item.EndDate.HasValue ? DateOnly.FromDateTime(item.EndDate.Value) : (DateOnly?)null
                );
            }

            // Lọc theo trạng thái nếu được yêu cầu
            if (!string.IsNullOrEmpty(searchRequest.Status))
            {
                list = list.Where(x => x.Status == searchRequest.Status).ToList();
            }

            return list;
        }

        public async Task<int> CountCoursesAsync(CourseSearchRequest searchRequest)
        {
            var query = _context.Courses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchRequest.CourseName))
            {
                var name = searchRequest.CourseName.Trim().ToLower();
                query = query.Where(c => c.CourseName != null && c.CourseName.ToLower().Contains(name));
            }

            if (searchRequest.LicenceTypeId.HasValue)
            {
                query = query.Where(c => c.LicenceTypeId == searchRequest.LicenceTypeId);
            }

            if (searchRequest.StartDate.HasValue)
            {
                var start = DateOnly.FromDateTime(searchRequest.StartDate.Value.Date);
                query = query.Where(c => !c.StartDate.HasValue || c.StartDate >= start);
            }

            if (searchRequest.EndDate.HasValue)
            {
                var end = DateOnly.FromDateTime(searchRequest.EndDate.Value.Date);
                query = query.Where(c => !c.EndDate.HasValue || c.EndDate <= end);
            }

            // Không áp trạng thái ở đây để tránh phải load dữ liệu; nếu cần chính xác theo trạng thái,
            // có thể materialize và đếm, nhưng tạm thời chấp nhận đếm theo các điều kiện cơ bản.
            return await query.CountAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses.Include(c => c.LicenceType).FirstOrDefaultAsync(c => c.CourseId == id);
        }

        public async Task<CourseDetailResponse?> GetDetailAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.LicenceType)
                .Include(c => c.Classes)
                    .ThenInclude(cl => cl.Instructor)
                .Include(c => c.Classes)
                    .ThenInclude(cl => cl.ClassMembers)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return null;
            }

            var response = new CourseDetailResponse
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                LicenceCode = course.LicenceType?.LicenceCode,
                StartDate = course.StartDate?.ToDateTime(TimeOnly.MinValue),
                EndDate = course.EndDate?.ToDateTime(TimeOnly.MinValue),
                TotalClasses = course.Classes.Count,
                TotalStudents = course.Classes.SelectMany(cl => cl.ClassMembers).Count()
            };

            response.Classes = course.Classes.Select(cl => new MappingObjects.Class.ClassListResponse
            {
                ClassId = cl.ClassId,
                ClassName = cl.ClassName,
                InstructorName = cl.Instructor != null ? ($"{cl.Instructor.FirstName} {cl.Instructor.MiddleName} {cl.Instructor.LastName}").Trim() : "Chưa phân công",
                LicenceCode = course.LicenceType?.LicenceCode,
                //StartDate/EndDate của lớp không có trong model hiện tại -> để trống
                TotalStudents = cl.ClassMembers.Count,
                Status = string.Empty
            }).ToList();

            response.Status = GetCourseStatus(course.StartDate, course.EndDate);
            return response;
        }

        public async Task<bool> CourseExistsAsync(int id)
        {
            return await _context.Courses.AnyAsync(c => c.CourseId == id);
        }

        public async Task<bool> CourseNameExistsAsync(string courseName, int? excludeCourseId = null)
        {
            if (string.IsNullOrWhiteSpace(courseName)) return false;
            var query = _context.Courses.AsQueryable();
            query = query.Where(c => c.CourseName != null && c.CourseName.ToLower() == courseName.Trim().ToLower());
            if (excludeCourseId.HasValue)
            {
                query = query.Where(c => c.CourseId != excludeCourseId.Value);
            }
            return await query.AnyAsync();
        }

        public async Task CreateAsync(Course entity)
        {
            // Assume uniqueness checked at service layer
            await _context.Courses.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Courses.Include(c => c.Classes).FirstOrDefaultAsync(c => c.CourseId == id);
            if (entity == null)
            {
                return;
            }

            if (entity.Classes.Any())
            {
                // Chặn xóa nếu đã có lớp
                throw new InvalidOperationException("Không thể xóa khóa học đã có lớp.");
            }

            _context.Courses.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LicenceType>> GetAllLicenceTypesAsync()
        {
            return await _context.LicenceTypes.ToListAsync();
        }

        public async Task UpdateAsync(Course entity)
        {
            var existing = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == entity.CourseId);
            if (existing == null)
            {
                return;
            }

            existing.CourseName = entity.CourseName;
            existing.LicenceTypeId = entity.LicenceTypeId;
            existing.StartDate = entity.StartDate;
            existing.EndDate = entity.EndDate;

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetNextCourseSequenceAsync(byte licenceTypeId)
        {
            // Lấy số thứ tự tiếp theo theo từng loại bằng để tránh nhảy lung tung giữa các loại
            var countByType = await _context.Courses.CountAsync(c => c.LicenceTypeId == licenceTypeId);
            return countByType + 1;
        }

        public async Task<IEnumerable<CourseWithStudentCountResponse>> GetCoursesWithStudentCountAsync()
        {
            var coursesWithStudentCount = await _context.Courses
                .Include(c => c.LicenceType)
                .OrderByDescending(c => c.CourseId)
                .Select(c => new CourseWithStudentCountResponse
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName ?? ("KH-" + c.CourseId),
                    LicenceTypeId = c.LicenceTypeId,
                    StudentCount = _context.LearningApplications
                        .Count(la => la.LicenceTypeId == c.LicenceTypeId &&
                                    la.Learner.UserRoles.Any(ur => ur.RoleId == 1))   // learner role

                })
                .ToListAsync();

            return coursesWithStudentCount;
        }

        public async Task<CourseInfoResponse?> GetCourseInfoAsync(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.LicenceType)
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
            {
                return null;
            }

            // Count students waiting for this course (based on license type)
            var studentCount = await _context.LearningApplications
                .CountAsync(la => la.LicenceTypeId == course.LicenceTypeId &&
                                 la.Learner.UserRoles.Any(ur => ur.RoleId == 1) && // learner role
                                 la.LearningStatus != 4 && // not completed
                                 !la.ClassMembers.Any()); // not assigned to any class yet

            return new CourseInfoResponse
            {
                CourseName = course.CourseName ?? ("KH-" + course.CourseId),
                LicenceType = course.LicenceType?.LicenceCode ?? "Không xác định",
                LicenceTypeName = course.LicenceType?.LicenceCode ?? "Không xác định",
                StudentCount = studentCount
            };
        }

        private static string GetCourseStatus(DateOnly? startDate, DateOnly? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return "Chưa xác định";
            }

            var today = DateOnly.FromDateTime(DateTime.Now);
            if (today < startDate)
            {
                return "Chưa bắt đầu";
            }
            if (today >= startDate && today <= endDate)
            {
                return "Đang diễn ra";
            }
            return "Đã kết thúc";
        }
    }
}

