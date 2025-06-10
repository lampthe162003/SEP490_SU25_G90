using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository
{
    public interface ITestApplicationRepository
    {
        List<TestApplication> GetAllTestApplication(); 
        List<TestApplication> GetByName(string name); 
        List<TestApplication> GetByCccd(string cccd); 
    }
}
