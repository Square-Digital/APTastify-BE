using Microsoft.Extensions.Logging;

namespace AP.BusinessLogic.Services.Email;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly ILogger<EmailTemplateService> _logger;

    public EmailTemplateService(ILogger<EmailTemplateService> logger)
    {
        _logger = logger;
    }

    private readonly Dictionary<string, string> _templates = new()
    {
        ["SignupEmail"] = """
                          <!DOCTYPE html>
                          <html>
                          <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333333; max-width: 600px; margin: 0 auto; padding: 20px;">
                              <div style="background-color: #ffffff; padding: 20px; border-radius: 5px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                                  <h1 style="color: #2c3e50; margin-bottom: 20px;">Welcome to APTastify!</h1>
                                  
                                  <p>Dear {{Email}},</p>
                                  
                                  <p>Thank you for signing up! We're excited to have you on board.</p>
                                  
                                  <p>To get started, please verify your email address by clicking the button below:</p>
                                  
                                  <div style="text-align: center; margin: 30px 0;">
                                      <a href="{{ConfirmationLink}}" style="background-color: #3498db; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; display: inline-block;">Verify Email Address</a>
                                  </div>
                                  
                                  <p>If the button above doesn't work, you can copy and paste this link into your browser:</p>
                                  <p style="font-size: 12px; color: #666;">{{ConfirmationLink}}</p>
                                  
                                  <p>If you didn't create an account, you can safely ignore this email.</p>
                                  
                                  <div style="margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee;">
                                      <p style="font-size: 12px; color: #666;">
                                          Best regards,<br>
                                          The APTastify Team
                                      </p>
                                  </div>
                              </div>
                          </body>
                          </html>
                          """
    };

    public Task<string> RenderTemplateAsync<T>(string templateName, T model)
    {
        if (!_templates.TryGetValue(templateName, out var template))
        {
            throw new ArgumentException($"Template '{templateName}' not found.");
        }

        _logger.LogInformation("Rendering template {TemplateName} with model type {ModelType}", templateName, typeof(T).Name);
        
        var renderedTemplate = template;
        foreach (var prop in typeof(T).GetProperties())
        {
            var value = prop.GetValue(model)?.ToString() ?? string.Empty;
            _logger.LogInformation("Replacing {{{{{0}}}}} with value: {1}", prop.Name, value);
            renderedTemplate = renderedTemplate.Replace($"{{{{{prop.Name}}}}}", value);
        }

        return Task.FromResult(renderedTemplate);
    }

    public Task<IEnumerable<string>> GetAvailableTemplatesAsync()
    {
        return Task.FromResult(_templates.Keys.AsEnumerable());
    }

    public Task<bool> TemplateExistsAsync(string templateName)
    {
        return Task.FromResult(_templates.ContainsKey(templateName));
    }
}