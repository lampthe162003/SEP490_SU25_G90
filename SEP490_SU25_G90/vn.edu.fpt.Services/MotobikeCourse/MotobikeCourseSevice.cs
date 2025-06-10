using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.MotobikeCouseRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.Course
{
    public class MotobikeCourseService : IMotobikeCourseService
    {
        private readonly IMotobikeCourseRepository _motobikeCourseRepository;
        private readonly IMapper _mapper;

        public MotobikeCourseService(IMotobikeCourseRepository motobikeCourseRepository, IMapper mapper)
        {
            _motobikeCourseRepository = motobikeCourseRepository;
            _mapper = mapper;
        }

        public List<CourseInformationResponse> GetAllMotobikeCourse()
        {
            var courses = _motobikeCourseRepository.GetAllMotobikeCourse();
            return _mapper.Map<List<CourseInformationResponse>>(courses);
        }
    }
}
