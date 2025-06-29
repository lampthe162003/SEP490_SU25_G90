using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.TestScoreStandardRepository
{
    public class TestScoreStandardRepository : ITestScoreStandardRepository
    {
        readonly Sep490Su25G90DbContext _context;
        public TestScoreStandardRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }
        public IQueryable<TestScoreStandard> GetAll()
        {
            return _context.TestScoreStandards.AsQueryable();
        }
    }
}
