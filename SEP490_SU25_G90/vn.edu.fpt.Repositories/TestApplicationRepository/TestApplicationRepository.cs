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

        public async Task<List<TestApplication>> GetAllTestApplicationAsync()
        {
            return await _context.TestApplications
                .Include(t => t.Learning)
                    .ThenInclude(la => la.Learner)
                        .ThenInclude(u => u.Cccd)
                .Include(t => t.Learning)
                    .ThenInclude(la => la.LicenceType)
                .ToListAsync();
        }

        public async Task<List<TestApplication>> GetByNameAsync(string name)
        {
            return await _context.TestApplications
                .Include(t => t.Learning)
                    .ThenInclude(la => la.Learner)
                        .ThenInclude(u => u.Cccd)
                .Include(t => t.Learning)
                    .ThenInclude(la => la.LicenceType)
                .Where(t => t.Learning.Learner.FirstName.Contains(name) ||
                            t.Learning.Learner.MiddleName.Contains(name) ||
                            t.Learning.Learner.LastName.Contains(name))
                .ToListAsync();
        }

        public async Task<List<TestApplication>> GetByCccdAsync(string cccd)
        {
            return await _context.TestApplications
                .Include(t => t.Learning)
                    .ThenInclude(la => la.Learner)
                        .ThenInclude(u => u.Cccd)
                .Include(t => t.Learning)
                    .ThenInclude(la => la.LicenceType)
                .Where(t => t.Learning.Learner.Cccd.CccdNumber == cccd)
                .ToListAsync();
        }

        public async Task<TestApplication> Create(TestApplication testApplication)
        {
            testApplication = _context.TestApplications.Add(testApplication).Entity;
            await _context.SaveChangesAsync();
            return testApplication;
        }
    }



}
