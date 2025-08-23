using Microsoft.Extensions.Configuration;

namespace Flashcards.Config;
public sealed class AppConfig
{
    private static readonly Lazy<IConfigurationRoot> _instance =
        new Lazy<IConfigurationRoot>(() => BuildConfiguation());

    private AppConfig () { }

    public static IConfigurationRoot Instance => _instance.Value;

    private static IConfigurationRoot BuildConfiguation()
    {
        try
        {
            return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        }
        catch (FileNotFoundException ex)
        {
            Console.Error.WriteLine($"Missing configuration file: {ex.Message}");
            throw new InvalidOperationException("Configuration file (appsettings.json) is required.", ex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            throw new InvalidOperationException("Failed to build configuration.", ex);
        }
    }
}
