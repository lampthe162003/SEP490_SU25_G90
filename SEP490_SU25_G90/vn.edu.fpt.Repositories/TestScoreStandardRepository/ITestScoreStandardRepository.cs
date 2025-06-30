using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.TestScoreStandardRepository
{
    public interface ITestScoreStandardRepository
    {
        IQueryable<TestScoreStandard> GetAll();
    }
}
