using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
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

        public IQueryable<TestApplication> GetAll()
        {
            return _context.TestApplications.AsQueryable();
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
        public async Task<TestApplication> Create(TestApplication testApplication)
        {
            testApplication = _context.TestApplications.Add(testApplication).Entity;
            await _context.SaveChangesAsync();
            return testApplication;
        }

        public async Task<TestApplication> Update(TestApplication testApplication)
        {
            _context.Entry(testApplication).CurrentValues.SetValues(testApplication);
            await _context.SaveChangesAsync();
            return testApplication;
        }
    }



}
