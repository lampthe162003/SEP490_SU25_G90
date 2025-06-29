using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication
{
    public class TestApplicationService : ITestApplicationService
    {
        private readonly ITestApplicationRepository _testApplicationRepository;
        private readonly IMapper _mapper;

        public TestApplicationService(ITestApplicationRepository testApplicationRepository, IMapper mapper)
        {
            _testApplicationRepository = testApplicationRepository;
            _mapper = mapper;
        }

        public async Task<Models.TestApplication> CreateTestApplication(CreateTestApplicationRequest request)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "test-application");
            Directory.CreateDirectory(path);
            if (request.Attachment != null)
            {
                path = Path.Combine(path, request.Attachment.FileName);
                using (FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                {
                    await request.Attachment.CopyToAsync(fs);
                }
            }
            Models.TestApplication testApplication = new Models.TestApplication()
            {
                ExamDate = request.ExamDate,
                LearningId = request.LearningApplicationId,
                Notes = request.Note,
                ObstacleScore = request.ObstacleScore,
                PracticalScore = request.PracticalScore,
                SimulationScore = request.SimulationScore,
                TheoryScore = request.TheoryScore,
                Status = null,
                ResultImageUrl = request.Attachment == null ? null : path
            };
            return await _testApplicationRepository.Create(testApplication);
        }

        public async Task<List<TestApplicationListInformationResponse>> GetAllTestApplicationAsync()
        {
            var list = await _testApplicationRepository.GetAllTestApplicationAsync();
            return _mapper.Map<List<TestApplicationListInformationResponse>>(list);
        }

        public async Task<List<TestApplicationListInformationResponse>> GetByCccdAsync(string cccd)
        {
            var list = await _testApplicationRepository.GetByCccdAsync(cccd);
            return _mapper.Map<List<TestApplicationListInformationResponse>>(list);
        }

        public async Task<List<TestApplicationListInformationResponse>> GetByNameAsync(string name)
        {
            var list = await _testApplicationRepository.GetByNameAsync(name);
            return _mapper.Map<List<TestApplicationListInformationResponse>>(list);
        }
    }
}
