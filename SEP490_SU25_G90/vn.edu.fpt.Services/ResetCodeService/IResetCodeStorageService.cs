namespace SEP490_SU25_G90.vn.edu.fpt.Services.ResetCodeService
{
    public interface IResetCodeStorageService
    {
        void SaveCode(string email, string code, TimeSpan expiresIn);
        (string Code, DateTime ExpiresAt)? GetCode(string email);
        void RemoveCode(string email);
    }
}
