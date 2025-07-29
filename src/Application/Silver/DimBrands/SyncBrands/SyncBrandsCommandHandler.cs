
using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Silver.DimBrands.SyncBrands;

internal sealed class SyncBrandsCommandHandler(
    IDbConnectionFactory dbConnectionFactory) : ICommandHandler<SyncBrandsCommand>
{
    public async Task<Result> Handle(SyncBrandsCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            -- UPSERT
            INSERT INTO silver.dim_brands (
                brand_id, 
                brand_name
            )
            SELECT
                b.brand_id, 
                b.brand_name
            FROM bronze.brands b
            ON CONFLICT (brand_id) DO UPDATE SET
                brand_name = EXCLUDED.brand_name;

            -- DELETE stale rows
            DELETE FROM silver.dim_brands db
            WHERE NOT EXISTS (
                SELECT 1 
                FROM bronze.brands b
                WHERE db.brand_id = b.brand_id
            );
            """;

        await connection.ExecuteAsync(sql, cancellationToken);

        return Result.Success();
    }
}
