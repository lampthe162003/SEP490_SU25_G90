namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.LicenseTypeRepository
{
    public interface ILicenseTypeRepository
    {
        IQueryable<Models.LicenceType> GetAll();
    }
}
