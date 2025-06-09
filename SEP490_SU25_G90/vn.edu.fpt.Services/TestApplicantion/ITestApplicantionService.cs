using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestApplicantion
{
    public interface ITestApplicantionService
    {
        public List<TestApplicantionListInformationResponse> GetAll();
        List<TestApplicantionListInformationResponse> GetByName(string name);
        List<TestApplicantionListInformationResponse> GetByCccd(string cccd);
    }
}
