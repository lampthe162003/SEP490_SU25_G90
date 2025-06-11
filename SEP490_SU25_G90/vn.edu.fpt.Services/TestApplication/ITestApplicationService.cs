using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication
{
    public interface ITestApplicationService
    {
        public List<TestApplicationListInformationResponse> GetAllTestApplication();
        List<TestApplicationListInformationResponse> GetByName(string name);
        List<TestApplicationListInformationResponse> GetByCccd(string cccd);
    }
}
