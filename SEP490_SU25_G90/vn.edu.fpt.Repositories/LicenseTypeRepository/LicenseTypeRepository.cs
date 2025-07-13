using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.LicenseTypeRepository
{
    public class LicenseTypeRepository : ILicenseTypeRepository
    {
        readonly Sep490Su25G90DbContext sep490Su25G90DbContext;
        public LicenseTypeRepository(Sep490Su25G90DbContext sep490Su25G90DbContext) { 
            this.sep490Su25G90DbContext = sep490Su25G90DbContext;
        }
        public IQueryable<LicenceType> GetAll()
        {
            return sep490Su25G90DbContext.LicenceTypes.AsQueryable();
        }
    }
}
