using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication
{
    public class TestApplicationService : ITestApplicationService
    {
        private readonly ITestApplicationRepository _testApplicatRepository ;
        private readonly IMapper _mapper;

        public TestApplicationService(Sep490Su25G90DbContext context, IMapper mapper)
        {
            _testApplicatRepository = new TestApplicationRepository(context);
            _mapper = mapper;
        }

        public List<TestApplicationListInformationResponse> GetAllTestApplication()
        => _mapper.Map<List<TestApplicationListInformationResponse>>(_testApplicatRepository.GetAllTestApplication());


        public List<TestApplicationListInformationResponse> GetByCccd(string cccd)
        => _mapper.Map < List < TestApplicationListInformationResponse >>(_testApplicatRepository.GetByCccd(cccd));

        public List<TestApplicationListInformationResponse> GetByName(string name)
         => _mapper.Map<List<TestApplicationListInformationResponse>>(_testApplicatRepository.GetByName(name));
    }
}
