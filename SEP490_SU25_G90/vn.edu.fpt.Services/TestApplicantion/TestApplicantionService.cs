using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicantionRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestApplicantion
{
    public class TestApplicantionService : ITestApplicantionService
    {
        private readonly ITestApplicantionRepository _testApplicantRepository ;
        private readonly IMapper _mapper;

        public TestApplicantionService(Sep490Su25G90DbContext context, IMapper mapper)
        {
            _testApplicantRepository = new TestApplicantionRepository(context);
            _mapper = mapper;
        }

        public List<TestApplicantionListInformationResponse> GetAll()
        => _mapper.Map<List<TestApplicantionListInformationResponse>>(_testApplicantRepository.GetAll());


        public List<TestApplicantionListInformationResponse> GetByCccd(string cccd)
        => _mapper.Map < List < TestApplicantionListInformationResponse >>(_testApplicantRepository.GetByCccd(cccd));

        public List<TestApplicantionListInformationResponse> GetByName(string name)
         => _mapper.Map<List<TestApplicantionListInformationResponse>>(_testApplicantRepository.GetByName(name));
    }
}
