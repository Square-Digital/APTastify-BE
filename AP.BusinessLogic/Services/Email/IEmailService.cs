namespace AP.BusinessLogic.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlContent);
    Task SendTemplatedEmailAsync<T>(string to, string templateName, T model);

}