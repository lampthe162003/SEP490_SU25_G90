using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicantionRepository
{
    public interface ITestApplicantionRepository
    {
        List<TestApplication> GetAll(); 
        List<TestApplication> GetByName(string name); 
        List<TestApplication> GetByCccd(string cccd); 
    }
}
