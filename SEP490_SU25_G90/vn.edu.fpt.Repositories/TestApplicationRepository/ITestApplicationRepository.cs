using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository
{
    public interface ITestApplicationRepository
    {
        Task<List<TestApplication>> GetAllTestApplicationAsync();
        Task<List<TestApplication>> GetByNameAsync(string name);
        Task<List<TestApplication>> GetByCccdAsync(string cccd);
    }
}
