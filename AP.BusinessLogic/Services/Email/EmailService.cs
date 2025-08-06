using AP.BusinessInterfaces.Data.Email;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AP.BusinessLogic.Services.Email;

public class EmailService(
    IOptions<EmailSettings> emailSettings,
    IEmailTemplateService templateService,
    ILogger<EmailService> logger)
    : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly IEmailTemplateService _templateService = templateService;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string to, string subject, string htmlContent)
    {
        try
        {
            _logger.LogInformation("Attempting to send email to {To} with subject {Subject}", to, subject);
            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailSettings.FromEmail));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = htmlContent
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            _logger.LogInformation("Connecting to SMTP server {Server}:{Port}", _emailSettings.SmtpServer, _emailSettings.SmtpPort);
            
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            
            _logger.LogInformation("Email sent successfully to {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}. Error: {Message}", to, ex.Message);
            throw;
        }
    }

    public async Task SendTemplatedEmailAsync<T>(string to, string templateName, T model)
    {
        try
        {
            _logger.LogInformation("Rendering template {TemplateName} for {To}", templateName, to);
            var htmlContent = await _templateService.RenderTemplateAsync(templateName, model);
            await SendEmailAsync(to, GetSubjectForTemplate(templateName), htmlContent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send templated email {TemplateName} to {To}", templateName, to);
            throw;
        }
    }
    
    private string GetSubjectForTemplate(string templateName)
    {
        // You could make this more sophisticated by storing subjects in configuration
        return templateName switch
        {
            "Welcome" => "Welcome to Our Service",
            "PasswordReset" => "Password Reset Request",
            "AccountConfirmation" => "Please Confirm Your Account",
            "SignupEmail" => "Welcome to APTastify!",
            _ => "Notification"
        };
    }
}