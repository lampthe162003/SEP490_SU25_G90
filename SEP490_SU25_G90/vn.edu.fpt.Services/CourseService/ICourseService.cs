using System.Collections.Generic;
using System.Threading.Tasks;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CourseService
{
    public interface ICourseService
    {
        Task<Pagination<CourseListResponse>> GetCoursesAsync(CourseSearchRequest searchRequest);
        Task<IEnumerable<LicenceType>> GetAllLicenceTypesAsync();
        Task<Course?> GetDetailAsync(int id);
        Task<MappingObjects.Course.CourseDetailResponse?> GetCourseDetailAsync(int id);
        Task<bool> CourseExistsAsync(int id);
        Task CreateAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task<bool> CourseNameExistsAsync(string courseName, int? excludeCourseId = null);
        Task<string> GenerateCourseNameAsync(byte licenceTypeId);
    }
}

