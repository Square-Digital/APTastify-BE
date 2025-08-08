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
        AddCors(services);
        
        services.AddControllers();
    }

    public static async Task<Dictionary<string, string?>> GetAwsConfigurationSecrets(this IServiceCollection services,
        string? access, string? secret,
        bool development = false)
    {
        try
        {
            APSecretsManager awsSecretsManager;

            if (string.IsNullOrWhiteSpace(access) || string.IsNullOrWhiteSpace(secret))
            {
                awsSecretsManager = new APSecretsManager();
            }
            else
            {
                awsSecretsManager = new APSecretsManager(access, secret);
            }

            var secrets = await GetAwsConfigurationSecrets(development, awsSecretsManager);

            var configuration = new Dictionary<string, string?>();

            ConfigureSettings(services, secrets, configuration);

            return configuration;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to get AWS secrets: {ex.Message}");
            Console.WriteLine("Falling back to local configuration...");
            return new Dictionary<string, string?>();
        }
    }

    private static void AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, policyBuilder =>
            {
                policyBuilder.WithOrigins(APTastifyUIALB, LocalDev)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
            });
        });
    }

    private static void ConfigureSettings(this IServiceCollection services, Dictionary<string, string?> secrets,
        Dictionary<string, string?> configuration)
    {
        if (secrets.TryGetValue(Connection, out var connection))
        {
            configuration["ConnectionStrings:DefaultConnection"] = connection;
        }

        // Map email settings to the proper configuration structure
        if (secrets.TryGetValue(SmtpServer, out var server))
        {
            configuration["EmailSettings:SmtpServer"] = server;
        }

        if (secrets.TryGetValue(SmtpPort, out var port))
        {
            configuration["EmailSettings:SmtpPort"] = port;
        }

        if (secrets.TryGetValue(FromEmail, out var fromEmail))
        {
            configuration["EmailSettings:FromEmail"] = fromEmail;
        }

        if (secrets.TryGetValue(FromName, out var fromName))
        {
            configuration["EmailSettings:FromName"] = fromName;
        }

        if (secrets.TryGetValue(SmtpUsername, out var username))
        {
            configuration["EmailSettings:Username"] = username;
        }

        if (secrets.TryGetValue(SmtpPassword, out var password))
        {
            configuration["EmailSettings:Password"] = password;
        }
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

        var configuration = new Dictionary<string, string?>();

        // Safely get values with null checks
        if (secrets.ContainsKey(connectionKey))
        {
            configuration[Connection] = secrets[connectionKey];
        }

        if (secrets.ContainsKey(SmtpPassword))
        {
            configuration[SmtpPassword] = secrets[SmtpPassword];
        }

        if (secrets.ContainsKey(SmtpPort))
        {
            configuration[SmtpPort] = secrets[SmtpPort];
        }

        if (secrets.ContainsKey(SmtpServer))
        {
            configuration[SmtpServer] = secrets[SmtpServer];
        }

        if (secrets.ContainsKey(SmtpUser))
        {
            configuration[SmtpUser] = secrets[SmtpUser];
        }

        if (secrets.ContainsKey(SmtpUsername))
        {
            configuration[SmtpUsername] = secrets[SmtpUsername];
        }

        if (secrets.ContainsKey(FromEmail))
        {
            configuration[FromEmail] = secrets[FromEmail];
        }

        if (secrets.ContainsKey(FromName))
        {
            configuration[FromName] = secrets[FromName];
        }

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