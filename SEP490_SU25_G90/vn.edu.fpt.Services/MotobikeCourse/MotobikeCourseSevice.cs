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

        public async Task<List<CourseInformationResponse>> GetAllMotobikeCourseAsync(string? search)
        {
            var data = await _motobikeCourseRepository.GetAllMotobikeCourseAsync(search);
            return _mapper.Map<List<CourseInformationResponse>>(data);
        }

    }
}
