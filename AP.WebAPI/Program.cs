using AP.WebAPI.DependencyInjection;
using AP.WebAPI.RequestPipeline;
using static AP.BusinessInterfaces.Libs.Constants;

var builder = WebApplication.CreateBuilder(args);

// Get environment and AWS credentials from builder configuration
var environment = builder.Configuration["ENV"] ?? Development;
var access = builder.Configuration["AWS:Access"] ?? string.Empty;
var secret = builder.Configuration["AWS:Secret"] ?? string.Empty;

Console.WriteLine($"Environment: {environment}");

// Get AWS configuration secrets (will fall back to local config if AWS fails)
var configurations = await builder.Services.GetAwsConfigurationSecrets(access, secret, environment == Development);

// Add the AWS configurations to the builder configuration
foreach (var config in configurations.Where(config => config.Value != null))
{
    builder.Configuration[config.Key] = config.Value;
}

// Ensure we have a connection string - use local config if AWS didn't provide one
if (string.IsNullOrEmpty(builder.Configuration.GetConnectionString("DefaultConnection")))
{
    var localConnection = builder.Configuration["ConnectionStrings:DefaultConnection"];
    if (!string.IsNullOrEmpty(localConnection))
    {
        builder.Configuration["ConnectionStrings:DefaultConnection"] = localConnection;
    }
}

// Add services with the updated configuration
builder.Services.AddServices(builder.Configuration, environment == Development);

var app = builder.Build();
app.InitializeApplication();

await app.RunAsync();