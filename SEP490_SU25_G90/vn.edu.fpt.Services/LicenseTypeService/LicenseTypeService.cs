
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LicenseTypeRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.LicenseTypeService
{
    public class LicenseTypeService : ILicenseTypeService
    {
        private readonly ILicenseTypeRepository repository;
        public LicenseTypeService(ILicenseTypeRepository repository)
        {
            this.repository = repository;
        }
        public List<(int id, string type)> GetKeyValues()
        {
            return repository.GetAll()
                .ToList()
                .Select(x =>
                {
                    return ((int)x.LicenceTypeId, x.LicenceCode);
                })
                .ToList();
        }
    }
}
