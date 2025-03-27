using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailMessageDTO emailMessage)
        {
            using var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Email"],
                    _configuration["EmailSettings:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:Email"]),
                ReplyToList = { new MailAddress(emailMessage.ToEmail) },
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                //IsBodyHtml = emailMessage.IsHtml,
            };

            mailMessage.To.Add(emailMessage.ToEmail);

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }

}
