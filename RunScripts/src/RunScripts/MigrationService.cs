using System.Data;
using System.Globalization;
using System.Resources;
using Dapper;
using Npgsql;
using System;

namespace RunScripts;

public class MigrationService
{
    private readonly string _connectionString;
    private readonly string _migrationsFolder;

    public MigrationService(AppConfig config)
    {
        _connectionString = config.ConnectionString;
        _migrationsFolder = config.MigrationsFolder;
    }

    public async Task MigrateDatabaseAsync()
    {
        await InitializeDatabaseAsync();

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        try
        {
            await CreateSchemaForMigrationTableIfNotExists(connection);
            await CreateMigrationsTableIfNotExists(connection);

            List<string> appliedMigrations = await GetAppliedMigrations(connection);
            List<Migration> allMigrations = GetMigrationsFromFiles();

            foreach (Migration migration in allMigrations)
            {
                if (!appliedMigrations.Contains(migration.Name))
                {
                    try
                    {
                        await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();
                        await ApplyMigration(migration, connection, transaction);
                        await transaction.CommitAsync();

                        Console.WriteLine($"Migration {migration.Name} applied successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error applying migration {migration.Name}: {ex.Message}");
                        throw;
                    }
                }
            }

#pragma warning disable CA1303 // Do not pass literals as localized parameters
            Console.WriteLine("Database Migration Process Completed");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        }
        catch (Exception ex)
        {
            Console.WriteLine($"A critical error occurred during migration: {ex.Message}");
            throw;
        }
    }

    private async Task InitializeDatabaseAsync()
    {
        string? databaseName = new NpgsqlConnectionStringBuilder(_connectionString).Database;

        string masterConnectionString = new NpgsqlConnectionStringBuilder(_connectionString)
        {
            Database = "postgres"
        }.ConnectionString;

        using var connection = new NpgsqlConnection(masterConnectionString);

        await connection.OpenAsync();

        bool databaseExists = await connection.ExecuteScalarAsync<bool>(
            "SELECT EXISTS(SELECT datname FROM pg_catalog.pg_database WHERE datname = @databaseName)",
            new { databaseName });

        if (!databaseExists)
        {
            await connection.ExecuteAsync($"CREATE DATABASE \"{databaseName}\"");
            Console.WriteLine($"Database \"{databaseName}\" created successfully.");
        }
    }

    private async Task CreateSchemaForMigrationTableIfNotExists(IDbConnection connection)
    {
        const string createSchemaSql = "CREATE SCHEMA IF NOT EXISTS sys;";
        await connection.ExecuteAsync(createSchemaSql);
    }

    private async Task CreateMigrationsTableIfNotExists(IDbConnection connection)
    {
        const string createTableSql = @"
                CREATE TABLE IF NOT EXISTS sys.migrations (
                    id SERIAL PRIMARY KEY,
                    name VARCHAR(255) NOT NULL,
                    applied_on TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                );";
        await connection.ExecuteAsync(createTableSql);
    }

    private async Task<List<string>> GetAppliedMigrations(IDbConnection connection)
    {
        const string query = "SELECT name FROM sys.migrations";
        IEnumerable<string> appliedMigrations = await connection.QueryAsync<string>(query);
        return appliedMigrations.ToList();
    }

    private List<Migration> GetMigrationsFromFiles()
    {
        var migrations = new List<Migration>();
        IOrderedEnumerable<string> migrationFiles = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), _migrationsFolder), "*.sql").OrderBy(f => f);

        foreach (string file in migrationFiles)
        {
            string migrationName = Path.GetFileNameWithoutExtension(file);
            string migrationScript = File.ReadAllText(file);
            migrations.Add(new Migration(migrationName, migrationScript));
        }

        return migrations;
    }

    private async Task ApplyMigration(Migration migration, IDbConnection connection, IDbTransaction transaction)
    {
        await connection.ExecuteAsync(migration.Script, transaction: transaction);
        const string insertMigrationRecordSql = "INSERT INTO sys.migrations (name) VALUES (@Name)";
        await connection.ExecuteAsync(insertMigrationRecordSql, new { migration.Name }, transaction: transaction);
    }

    private sealed class Migration
    {
        public string Name { get; }
        public string Script { get; }

        public Migration(string name, string script)
        {
            Name = name;
            Script = script;
        }
    }
}
