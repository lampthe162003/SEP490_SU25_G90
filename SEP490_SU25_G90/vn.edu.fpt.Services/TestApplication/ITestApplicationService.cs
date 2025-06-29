using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication
{
    public interface ITestApplicationService
    {
        Task<List<TestApplicationListInformationResponse>> GetAllTestApplicationAsync();
        Task<List<TestApplicationListInformationResponse>> GetByNameAsync(string name);
        Task<List<TestApplicationListInformationResponse>> GetByCccdAsync(string cccd);
        Task<CreatUpdateTestApplicationRequest> FindById(int id);
        Task<Models.TestApplication> CreateTestApplication(CreatUpdateTestApplicationRequest request);
        Task<Models.TestApplication> UpdateTestApplication(int id, CreatUpdateTestApplicationRequest request, bool? status);
    }
}
