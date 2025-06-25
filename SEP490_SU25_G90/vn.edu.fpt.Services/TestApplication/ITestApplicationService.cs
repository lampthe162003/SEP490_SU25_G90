using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication
{
    public interface ITestApplicationService
    {
        Task<List<TestApplicationListInformationResponse>> GetAllTestApplicationAsync();
        Task<List<TestApplicationListInformationResponse>> GetByNameAsync(string name);
        Task<List<TestApplicationListInformationResponse>> GetByCccdAsync(string cccd);
    }
}
