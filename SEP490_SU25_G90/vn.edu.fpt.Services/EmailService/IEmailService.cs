namespace SEP490_SU25_G90.vn.edu.fpt.Services.EmailService
{
    public interface IEmailService
    {
        public Task SendResetCodeAsync(string receiver, string resetCode);
    }
}
