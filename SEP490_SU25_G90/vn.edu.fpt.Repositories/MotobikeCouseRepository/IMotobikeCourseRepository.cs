using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.MotobikeCouseRepository
{
    public interface IMotobikeCourseRepository
    {
        Task<List<Course>> GetAllMotobikeCourseAsync(string? search);
    }
}
