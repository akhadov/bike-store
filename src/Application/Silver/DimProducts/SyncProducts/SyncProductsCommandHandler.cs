
using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Silver.DimProducts.SyncProducts;

internal sealed class SyncProductsCommandHandler(
    IDbConnectionFactory dbConnectionFactory) : ICommandHandler<SyncProductsCommand>
{
    public async Task<Result> Handle(SyncProductsCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            -- UPSERT
            INSERT INTO silver.dim_products (
                product_id, 
                product_name, 
                brand_id, 
                category_id, 
                model_year, 
                list_price
            )
            SELECT
                p.product_id, 
                p.product_name, 
                p.brand_id, 
                p.category_id, 
                p.model_year, 
                p.list_price
            FROM bronze.products p
            ON CONFLICT (product_id) DO UPDATE SET
                product_name = EXCLUDED.product_name,
                brand_id = EXCLUDED.brand_id,
                category_id = EXCLUDED.category_id,
                model_year = EXCLUDED.model_year,
                list_price = EXCLUDED.list_price;

            -- DELETE stale rows
            DELETE FROM silver.dim_products dp
            WHERE NOT EXISTS (
                SELECT 1 
                FROM bronze.products p
                WHERE dp.product_id = p.product_id
            );
            """;

        await connection.ExecuteAsync(sql, cancellationToken);

        return Result.Success();
    }
}
