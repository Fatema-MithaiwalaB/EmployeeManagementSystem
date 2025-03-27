namespace EmployeeManagementSystem.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessageDTO emailMessage);
    }

}
