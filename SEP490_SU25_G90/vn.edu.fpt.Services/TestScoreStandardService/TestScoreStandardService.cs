using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.TestScoreStandardRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestScoreStandardService
{
    public class TestScoreStandardService : ITestScoreStandardService
    {
        readonly ITestScoreStandardRepository testScoreStandardRepository;
        public TestScoreStandardService(ITestScoreStandardRepository testScoreStandardRepository)
        {
            this.testScoreStandardRepository = testScoreStandardRepository;
        }
        public List<TestScoreStandard> FindByLicenseType(int licenseTypeId)
        {
            return testScoreStandardRepository
                .GetAll()
                .Where(x => x.LicenceTypeId == licenseTypeId)
                .ToList();
        }

        public List<TestScoreStandard> FindByLearningApplication(int learningApplicationid)
        {
            return testScoreStandardRepository
                .GetAll()
                .Include(x => x.LicenceType)
                .Where(x => x.LicenceType.LearningApplications.Any(x => x.LearningId == learningApplicationid))
                .ToList();
        }
    }
}
