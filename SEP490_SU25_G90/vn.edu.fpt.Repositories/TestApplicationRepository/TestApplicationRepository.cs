using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository
{
    public class TestApplicationRepository : ITestApplicationRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public TestApplicationRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public List<TestApplication> GetAllTestApplication()
        {
            return _context.TestApplications
                .Include(t => t.Learner)
                    .ThenInclude(l => l.Learner)
                        .ThenInclude(u => u.Cccd)
                .Include(t => t.Learner)
                    .ThenInclude(l => l.LicenceType)
                .ToList();
        }

        public List<TestApplication> GetByName(string name)
        {
            name = name.ToLower().Trim();

            return _context.TestApplications
                .Include(t => t.Learner)
                    .ThenInclude(l => l.Learner)
                        .ThenInclude(u => u.Cccd)
                .Include(t => t.Learner)
                    .ThenInclude(l => l.LicenceType)
                .Where(t =>
                    (t.Learner.Learner.FirstName + " " +
                     t.Learner.Learner.MiddleName + " " +
                     t.Learner.Learner.LastName).ToLower().Contains(name))
                .ToList();
        }


        public List<TestApplication> GetByCccd(string cccd)
        {
            return _context.TestApplications
                .Include(t => t.Learner)
                    .ThenInclude(l => l.Learner)
                        .ThenInclude(u => u.Cccd)
                .Include(t => t.Learner)
                    .ThenInclude(l => l.LicenceType)
                .Where(t => t.Learner.Learner.Cccd.CccdNumber.Contains(cccd))
                .ToList();
        }

    }
}
