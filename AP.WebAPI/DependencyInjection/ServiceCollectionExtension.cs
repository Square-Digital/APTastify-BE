using Microsoft.OpenApi.Models;
using static AP.BusinessInterfaces.Libs.Constants;

namespace AP.WebAPI.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        
        AddSwagger(services);

        services.AddControllers();
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