using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository
{
    public interface ITestApplicationRepository
    {
        Task<List<TestApplication>> GetAllTestApplicationAsync();
        IQueryable<TestApplication> GetAll();
        Task<TestApplication> Create(TestApplication testApplication);
        Task<TestApplication> Update(TestApplication testApplication);
        Task<int> BulkCreateAsync(List<TestApplication> testApplications);
    }
}
