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
                                  
                                  <p>Hi {{Email}},</p>
                                  
                                  <p>Congratulations — you've just joined an exclusive circle of early AP Tastify insiders.</p>
                                  
                                  <p>Only a limited number of people get early access to the marketplace that's redefining how local food, recipes, and restaurants connect.</p>
                                  
                                  <p>Before we unlock your spot, please confirm your email by clicking below:</p>
                                  
                                  <div style="text-align: center; margin: 30px 0;">
                                      <a href="{{ConfirmationLink}}" style="background-color: #3498db; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; display: inline-block;">Confirm My Spot</a>
                                  </div>
                                  
                                  <p>If the button doesn't work, paste this link into your browser:</p>
                                  <p style="font-size: 12px; color: #666;">{{ConfirmationLink}}</p>
                                  
                                  <p>This is your first step toward discovering and trading with the best local kitchens, chefs, and food creators — all in one place.</p>
                                  
                                  <p>If you didn't request access, you can safely ignore this email.</p>
                                  
                                  <div style="margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee;">
                                      <p style="font-size: 12px; color: #666;">
                                          Welcome to the future of taste,<br>
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