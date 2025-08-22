using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CourseRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Pagination<CourseListResponse>> GetCoursesAsync(CourseSearchRequest searchRequest)
        {
            if (searchRequest.PageNumber <= 0) searchRequest.PageNumber = 1;
            if (searchRequest.PageSize <= 0) searchRequest.PageSize = 10;

            if (searchRequest.StartDate.HasValue && searchRequest.EndDate.HasValue &&
                searchRequest.StartDate > searchRequest.EndDate)
            {
                // Reset khi khoảng ngày không hợp lệ
                searchRequest.StartDate = null;
                searchRequest.EndDate = null;
            }

            var data = await _courseRepository.GetCoursesAsync(searchRequest);
            var total = await _courseRepository.CountCoursesAsync(searchRequest);

            return new Pagination<CourseListResponse>
            {
                Data = data,
                Total = total,
                TotalPage = (int)Math.Ceiling((double)total / searchRequest.PageSize)
            };
        }

        public async Task<IEnumerable<LicenceType>> GetAllLicenceTypesAsync()
        {
            return await _courseRepository.GetAllLicenceTypesAsync();
        }

        public async Task<string> GenerateCourseNameAsync(byte licenceTypeId)
        {
            // Lấy code loại bằng
            var licence = (await _courseRepository.GetAllLicenceTypesAsync())
                .FirstOrDefault(l => l.LicenceTypeId == licenceTypeId);
            var licenceCode = licence?.LicenceCode ?? "";
            var seq = await _courseRepository.GetNextCourseSequenceAsync(licenceTypeId);
            // Format: BH{seq:D2}-{licenceCode}
            return $"BH{seq:D2}-{licenceCode}";
        }

        public async Task<Course?> GetDetailAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task<MappingObjects.Course.CourseDetailResponse?> GetCourseDetailAsync(int id)
        {
            return await _courseRepository.GetDetailAsync(id);
        }

        public async Task<bool> CourseExistsAsync(int id)
        {
            return await _courseRepository.CourseExistsAsync(id);
        }

        public async Task CreateAsync(Course course)
        {
            // Validate unique name
            if (!string.IsNullOrWhiteSpace(course.CourseName))
            {
                var exists = await _courseRepository.CourseNameExistsAsync(course.CourseName);
                if (exists)
                {
                    throw new InvalidOperationException("Tên khóa học đã tồn tại.");
                }
            }
            await _courseRepository.CreateAsync(course);
        }

        public async Task DeleteAsync(int id)
        {
            await _courseRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(Course course)
        {
            if (!string.IsNullOrWhiteSpace(course.CourseName))
            {
                var exists = await _courseRepository.CourseNameExistsAsync(course.CourseName, course.CourseId);
                if (exists)
                {
                    throw new InvalidOperationException("Tên khóa học đã tồn tại.");
                }
            }
            await _courseRepository.UpdateAsync(course);
        }

        public async Task<bool> CourseNameExistsAsync(string courseName, int? excludeCourseId = null)
        {
            return await _courseRepository.CourseNameExistsAsync(courseName, excludeCourseId);
        }

        public async Task<IEnumerable<CourseWithStudentCountResponse>> GetCoursesWithStudentCountAsync()
        {
            return await _courseRepository.GetCoursesWithStudentCountAsync();
        }

        public async Task<CourseInfoResponse?> GetCourseInfoAsync(int courseId)
        {
            return await _courseRepository.GetCourseInfoAsync(courseId);
        }
    }
}

