using AP.WebAPI.DependencyInjection;
using AP.WebAPI.RequestPipeline;
using static AP.BusinessInterfaces.Libs.Constants;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var environment = configuration["ENV"] ?? Development;

Console.WriteLine($"Environment: {environment}");

var builder = WebApplication.CreateBuilder(args);

// Get AWS secrets

// Handle local environment connection string override
if (environment == Local)
{
    var connection = configuration.GetConnectionString("DefaultConnection")
                     ?? throw new InvalidOperationException("No Connection string provided.");
    configuration["ConnectionString"] = connection;
}


builder.Configuration.AddConfiguration(configuration);
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();
app.InitializeApplication();

await app.RunAsync();