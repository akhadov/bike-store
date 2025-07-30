using Microsoft.Extensions.Configuration;

namespace RunScripts;
public sealed class AppConfig
{
    public string ConnectionString { get; }
    public string MigrationsFolder { get; }

    public AppConfig()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        ConnectionString = config.GetConnectionString("Database")
                           ?? throw new Exception("Database connection string is missing in appsettings.json.");

        MigrationsFolder = config.GetValue<string>("MigrationSettings:MigrationsFolder")
                           ?? throw new Exception("Migrations folder path is missing in appsettings.json.");
    }

}
