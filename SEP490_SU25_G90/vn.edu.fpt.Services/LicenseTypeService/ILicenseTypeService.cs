namespace SEP490_SU25_G90.vn.edu.fpt.Services.LicenseTypeService
{
    public interface ILicenseTypeService
    {
        List<(int id, string type)> GetKeyValues();
    }
}
