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

        public async Task SendNewAccountPasswordAsync(string receiver, string fullName, string password)
        {
            string smtpHost = _configuration["Smtp:Host"];
            int smtpPort = int.Parse(_configuration["Smtp:Port"]);
            string smtpUser = _configuration["Smtp:Username"];
            string smtpPass = _configuration["Smtp:Password"];
            string fromEmail = _configuration["Smtp:From"];

            string htmlBody = $@"
                <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background-color: #0d6efd; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
                            .content {{ background-color: #f8f9fa; padding: 30px; border-radius: 0 0 10px 10px; }}
                            .password-box {{ background-color: #e7f3ff; border: 2px solid #0d6efd; padding: 15px; margin: 20px 0; border-radius: 5px; text-align: center; }}
                            .password {{ font-size: 24px; font-weight: bold; color: #0d6efd; letter-spacing: 2px; }}
                            .footer {{ margin-top: 20px; font-size: 12px; color: #666; text-align: center; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h2>🎉 Chào mừng bạn đến với hệ thống!</h2>
                            </div>
                            <div class='content'>
                                <p>Xin chào <strong>{fullName}</strong>,</p>
                                
                                <p>Chúc mừng! Tài khoản giảng viên của bạn đã được tạo thành công trong hệ thống trường dạy lái xe.</p>
                                
                                <p><strong>Thông tin đăng nhập:</strong></p>
                                <ul>
                                    <li><strong>Email:</strong> {receiver}</li>
                                    <li><strong>Mật khẩu:</strong></li>
                                </ul>
                                
                                <div class='password-box'>
                                    <div class='password'>{password}</div>
                                </div>
                                
                                <p><strong>⚠️ Quan trọng:</strong></p>
                                <ul>
                                    <li>Vui lòng đăng nhập và <strong>đổi mật khẩu</strong> ngay lần đầu tiên</li>
                                    <li>Không chia sẻ thông tin đăng nhập với bất kỳ ai</li>
                                    <li>Liên hệ bộ phận nhân sự nếu có bất kỳ thắc mắc nào</li>
                                </ul>
                                
                                <p>Cảm ơn bạn đã tham gia đội ngũ giảng viên của chúng tôi!</p>
                                
                                <div class='footer'>
                                    <p>Email này được gửi tự động từ hệ thống. Vui lòng không trả lời email này.</p>
                                </div>
                            </div>
                        </div>
                    </body>
                </html>";

            var message = new MailMessage(fromEmail, receiver)
            {
                Subject = "🔑 Tài khoản giảng viên mới - Thông tin đăng nhập",
                Body = htmlBody,
                IsBodyHtml = true
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
