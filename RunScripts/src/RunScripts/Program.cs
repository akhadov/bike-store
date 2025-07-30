using RunScripts;

var config = new AppConfig();

var migrationService = new MigrationService(config);

await migrationService.MigrateDatabaseAsync();
