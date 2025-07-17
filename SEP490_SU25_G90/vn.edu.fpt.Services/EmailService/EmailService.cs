using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Net;
using System.Net.Mail;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task SendResetCodeAsync(string receiver, string resetCode)
        {
            string smtpHost = _configuration["Smtp:Host"];
            int smtpPort = int.Parse(_configuration["Smtp:Port"]);
            string smtpUser = _configuration["Smtp:Username"];
            string smtpPass = _configuration["Smtp:Password"];
            string fromEmail = _configuration["Smtp:From"];

            var message = new MailMessage(fromEmail, receiver)
            {
                Subject = "Mã xác nhận quên mật khẩu",
                Body = $"Mã xác nhận của bạn là: {resetCode}",
                IsBodyHtml = false
            };

            using var smtpClient = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
