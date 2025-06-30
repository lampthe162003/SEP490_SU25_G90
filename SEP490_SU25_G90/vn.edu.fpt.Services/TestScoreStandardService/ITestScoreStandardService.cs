using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestScoreStandardService
{
    public interface ITestScoreStandardService
    {
        List<TestScoreStandard> FindByLicenseType(int licenseTypeId);

        List<TestScoreStandard> FindByLearningApplication(int learningApplicationid);
    }
}
