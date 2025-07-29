using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Silver.FactInventories.SyncInventories;

internal sealed class SyncInventoriesCommandHandler(
    IDbConnectionFactory dbConnectionFactory) : ICommandHandler<SyncInventoriesCommand>
{
    public async Task<Result> Handle(SyncInventoriesCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            -- UPSERT
            INSERT INTO silver.fact_inventory (
                store_id, 
                product_id, 
                quantity
            )
            SELECT
                s.store_id, 
                s.product_id, 
                s.quantity
            FROM bronze.stocks s
            ON CONFLICT (store_id, product_id) DO UPDATE SET
                quantity = EXCLUDED.quantity;

            -- DELETE stale rows
            DELETE FROM silver.fact_inventory fi
            WHERE NOT EXISTS (
                SELECT 1 
                FROM bronze.stocks s
                WHERE fi.store_id = s.store_id AND fi.product_id = s.product_id
            );
            """;

        await connection.ExecuteAsync(sql, cancellationToken);

        return Result.Success();
    }
}
