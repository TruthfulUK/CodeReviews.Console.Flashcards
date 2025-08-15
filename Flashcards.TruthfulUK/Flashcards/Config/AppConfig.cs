using Microsoft.Extensions.Configuration;

namespace Flashcards.Config;
internal class AppConfig
{
    private readonly IConfigurationRoot _configuration;

    public AppConfig()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        _configuration = builder.Build();
    }

    public string GetConnectionString()
    {
        return _configuration.GetConnectionString("DefaultConnection")
               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }
}
