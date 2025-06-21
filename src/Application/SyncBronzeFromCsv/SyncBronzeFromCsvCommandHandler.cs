using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using SharedKernel;

namespace Application.SyncBronzeFromCsv;

internal sealed class SyncBronzeFromCsvCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncBronzeFromCsvCommand>
{
    public async Task<Result> Handle(SyncBronzeFromCsvCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        DataTable table = csvService.ReadAsDataTableAsync(command.FilePath);

        string tableName = Path.GetFileNameWithoutExtension(command.FilePath);

        await EnsureBronzeTableExists(connection, table, tableName);

        foreach (DataRow row in table.Rows)
        {
            if (!await RowExists(connection, table, row, tableName))
            {
                await InsertRow(connection, table, row, tableName);
            }
        }

        return Result.Success();
    }

    private static async Task EnsureBronzeTableExists(IDbConnection connection, DataTable table, string name)
    {
        const string createSchemaSql = "CREATE SCHEMA IF NOT EXISTS bronze;";
        await connection.ExecuteAsync(createSchemaSql);

        var builder = new StringBuilder();
        builder.AppendLine(CultureInfo.InvariantCulture, $"CREATE TABLE IF NOT EXISTS bronze.{name} (");

        foreach (DataColumn col in table.Columns)
        {
            builder.AppendLine(CultureInfo.InvariantCulture, $"    \"{col.ColumnName}\" TEXT,");
        }

        // Remove the last comma and newline, then add closing parenthesis
        if (builder.Length > 0)
        {
            builder.Length -= Environment.NewLine.Length + 1; // Remove newline + comma
        }

        builder.AppendLine(");");

        string createTableSql = builder.ToString();
        await connection.ExecuteAsync(createTableSql);
    }

    private static async Task<bool> RowExists(IDbConnection connection, DataTable table, DataRow row, string tableName)
    {
        var parameters = new DynamicParameters();
        var where = new StringBuilder("WHERE ");
        int paramCount = 0;

        foreach (DataColumn col in table.Columns)
        {
            string paramName = $"param{paramCount}";
            object? value = row[col] == DBNull.Value ? null : row[col];

            if (value == null)
            {
                where.Append(CultureInfo.InvariantCulture, $"\"{col.ColumnName}\" IS NULL AND ");
            }
            else
            {
                where.Append(CultureInfo.InvariantCulture, $"\"{col.ColumnName}\" = @{paramName} AND ");
                parameters.Add(paramName, value);
            }

            paramCount++;
        }

        // Remove the last " AND "
        if (where.Length > 6)
        {
            where.Length -= 5;
        }

        string query = $"SELECT 1 FROM bronze.\"{tableName}\" {where} LIMIT 1;";
        object? result = await connection.ExecuteScalarAsync(query, parameters);
        return result is not null;
    }

    private static async Task InsertRow(IDbConnection connection, DataTable table, DataRow row, string tableName)
    {
        var parameters = new DynamicParameters();
        var columnNames = new List<string>();
        var parameterNames = new List<string>();
        int paramCount = 0;

        foreach (DataColumn col in table.Columns)
        {
            columnNames.Add($"\"{col.ColumnName}\"");
            string paramName = $"param{paramCount}";
            parameterNames.Add($"@{paramName}");

            object? value = row[col] == DBNull.Value ? null : row[col];
            parameters.Add(paramName, value);

            paramCount++;
        }

        string columns = string.Join(", ", columnNames);
        string values = string.Join(", ", parameterNames);
        string insertSql = $"INSERT INTO bronze.\"{tableName}\" ({columns}) VALUES ({values});";

        await connection.ExecuteAsync(insertSql, parameters);
    }
}
