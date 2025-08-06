using System.Text.Json;
using AP.AWS.Secrets;
using AP.BusinessInterfaces.Data.Email;
using AP.BusinessLogic.Services;
using AP.BusinessLogic.Services.Email;
using AP.BusinessLogic.Context;
using AP.WebAPI.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using static AP.BusinessInterfaces.Libs.Constants;
using static AP.WebAPI.Libs.Constants;

namespace AP.WebAPI.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration,
        bool development = true)
    {
        AddSwagger(services);
        AddDatabase(services, configuration);
        AddEmailServices(services, configuration);
        AddInternalServices(services);
        AddInternalManagers(services);

        services.AddControllers();
    }

    public static async Task<Dictionary<string, string?>> GetAwsConfigurationSecrets(this IServiceCollection services,
        string access, string secret,
        bool development = false)
    {
        var awsSecretsManager = new APSecretsManager(access, secret);
        ;
        var secrets = await GetAwsConfigurationSecrets(development, awsSecretsManager);

        var serializedDbConnections = JsonSerializer.Deserialize<string>(secrets[Connection]);
        var connection = string.IsNullOrWhiteSpace(serializedDbConnections) ? serializedDbConnections : string.Empty;


        return secrets;
    }

    private static async Task<Dictionary<string, string?>> GetAwsConfigurationSecrets(bool development,
        APSecretsManager secretsManager)
    {
        var connectionKey = development ? ConnectionDevelopment : ConnectionProduction;
        var secretKeys = new List<string>
        {
            ConnectionStrings,
            EmailSettingsProd,
        };

        var secrets = await secretsManager.GetSecrets(secretKeys);

        var configuration = new Dictionary<string, string?>
        {
            { Connection, secrets[connectionKey] },
            { SmtpPassword, secrets[SmtpPassword] },
            { SmtpPort, secrets[SmtpPort] },
            { SmtpServer, secrets[SmtpServer] },
            { SmtpUser, secrets[SmtpUser] },
            { SmtpUsername, secrets[SmtpUsername] },
            { FromEmail, secrets[FromEmail] },
            { FromName, secrets[FromName] }
        };

        return configuration;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Entity Framework with PostgreSQL
        services.AddDbContext<APTastifyDatabaseContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("AP.WebAPI")));
    }

    private static void AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure email settings
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

        // Register email services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
    }

    private static void AddInternalServices(this IServiceCollection services)
    {
        services.AddScoped<UserService>();
    }

    private static void AddInternalManagers(this IServiceCollection services)
    {
        services.AddScoped<UserManager>();
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(SwaggerApiVersion,
                new OpenApiInfo { Title = SwaggerApiTitle, Version = SwaggerApiVersion });
            c.AddSecurityDefinition(SwaggerAuthScheme, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = SwaggerAuthDescription,
                Name = SwaggerAuthName,
                Type = SecuritySchemeType.ApiKey,
                Scheme = SwaggerAuthScheme
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SwaggerAuthScheme
                        }
                    },
                    []
                }
            });
        });
    }
}