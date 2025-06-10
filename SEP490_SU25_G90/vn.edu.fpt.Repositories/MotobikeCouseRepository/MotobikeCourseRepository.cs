using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.MotobikeCouseRepository
{
    public class MotobikeCourseRepository : IMotobikeCourseRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public List<CourseInformationResponse> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
