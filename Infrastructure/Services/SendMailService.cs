using Application.Interfaces;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

    }
    public class SendMailService : ISendMailService
    {
        private readonly MailSettings mailSettings;

        private readonly ILogger<SendMailService> logger;

        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
        {
            mailSettings = _mailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            var currentYear = DateTime.Now.Year;

            var builder = new BodyBuilder
            {
                HtmlBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Xác nhận đặt phòng</title>
                    <style type='text/css'>
                        /* Inline CSS vì nhiều client email không hỗ trợ external CSS */
                        body {{
                            font-family: 'Helvetica Neue', Arial, sans-serif;
                            line-height: 1.6;
                            color: #333333;
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                        }}
                        .header {{
                            background-color: #4a6baf;
                            color: white;
                            padding: 20px;
                            text-align: center;
                            border-radius: 5px 5px 0 0;     
                        }}
                        .content {{
                            padding: 20px;
                            background-color: #f9f9f9;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 12px 24px;
                            background-color: #4a6baf;
                            color: white !important;
                            text-decoration: none;
                            border-radius: 4px;
                            font-weight: bold;
                            margin: 15px 0;
                        }}
                        .footer {{
                            text-align: center;
                            padding: 20px;
                            font-size: 12px;
                            color: #999999;
                        }}
                        @media only screen and (max-width: 600px) {{
                            /* Responsive design cho mobile */
                            body {{
                                padding: 10px;
                            }}
                        }}
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1>{subject}</h1>
                    </div>
                    <div class='content'>
                        <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>
                        <p>Mã đặt phòng của bạn: <strong>{htmlMessage}</strong></p>
                        <p>Đây là email tự động được gửi để xác nhận đơn đặt của bạn.</p>
                        <a href='#' class='button'>Xác nhận ngay</a>
                        <p>Nếu bạn không thực hiện hành động này, vui lòng bỏ qua email.</p>
                    </div>
                    <div class='footer'>
                        <p>© {currentYear} Travel Booking. All rights reserved.</p>
                        <p>
                            <a href='https://yourwebsite.com/privacy'>Chính sách bảo mật</a> | 
                            <a href='https://yourwebsite.com/contact'>Liên hệ</a>
                        </p>
                    </div>
                </body>
                </html>"
            };
            message.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await smtp.ConnectAsync(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                Directory.CreateDirectory("mailssave");
                var emailsavefile = $"mailssave/{Guid.NewGuid()}.eml";
                await message.WriteToAsync(emailsavefile);

                logger.LogWarning($"Gửi mail thất bại, lưu tại: {emailsavefile}");
                logger.LogError(ex, "Lỗi gửi mail");
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }

            logger.LogInformation("Đã gửi mail tới: " + email);
        }

    }
}
