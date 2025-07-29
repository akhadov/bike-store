using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Silver.DimCategories.SyncCategories;

internal sealed class SyncCategoriesCommandHandler(
    IDbConnectionFactory dbConnectionFactory) : ICommandHandler<SyncCategoriesCommand>
{
    public async Task<Result> Handle(SyncCategoriesCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            -- UPSERT
            INSERT INTO silver.dim_categories (
                category_id, 
                category_name
            )
            SELECT
                c.category_id, 
                c.category_name
            FROM bronze.categories c
            ON CONFLICT (category_id) DO UPDATE SET
                category_name = EXCLUDED.category_name;

            -- DELETE stale rows
            DELETE FROM silver.dim_categories dc
            WHERE NOT EXISTS (
                SELECT 1 
                FROM bronze.categories c
                WHERE dc.category_id = c.category_id
            );
            """;

        await connection.ExecuteAsync(sql, cancellationToken);

        return Result.Success();
    }
}
