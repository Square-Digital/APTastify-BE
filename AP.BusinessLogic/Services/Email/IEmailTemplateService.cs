namespace AP.BusinessLogic.Services.Email;

public interface IEmailTemplateService
{
    /// <summary>
    /// Renders an email template with the provided model
    /// </summary>
    /// <typeparam name="T">Type of the model used for the template</typeparam>
    /// <param name="templateName">Name of the template to render</param>
    /// <param name="model">Model containing the data for the template</param>
    /// <returns>The rendered HTML content of the email</returns>
    Task<string> RenderTemplateAsync<T>(string templateName, T model);

    /// <summary>
    /// Gets the available template names
    /// </summary>
    /// <returns>Collection of available template names</returns>
    Task<IEnumerable<string>> GetAvailableTemplatesAsync();

    /// <summary>
    /// Validates if a template exists
    /// </summary>
    /// <param name="templateName">Name of the template to check</param>
    /// <returns>True if template exists, false otherwise</returns>
    Task<bool> TemplateExistsAsync(string templateName);
}