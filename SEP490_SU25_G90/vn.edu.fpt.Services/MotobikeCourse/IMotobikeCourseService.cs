using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.Course
{
    public interface IMotobikeCourseService
    {
        Task<List<CourseInformationResponse>> GetAllMotobikeCourseAsync(string? search);
    }
}
