using System.Collections.Generic;
using System.Threading.Tasks;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CourseRepository
{
    /// <summary>
    /// Interface truy cập dữ liệu cho khóa học
    /// </summary>
    public interface ICourseRepository
    {
        Task<IEnumerable<CourseListResponse>> GetCoursesAsync(CourseSearchRequest searchRequest);
        Task<int> CountCoursesAsync(CourseSearchRequest searchRequest);
        Task<Course?> GetByIdAsync(int id);
        Task<CourseDetailResponse?> GetDetailAsync(int id);
        Task<bool> CourseExistsAsync(int id);
        Task<bool> CourseNameExistsAsync(string courseName, int? excludeCourseId = null);
        Task<int> GetNextCourseSequenceAsync(byte licenceTypeId);
        Task CreateAsync(Course entity);
        Task UpdateAsync(Course entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<LicenceType>> GetAllLicenceTypesAsync();
    }
}

