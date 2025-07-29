
using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Silver.DimStores.SyncStores;

internal sealed class SyncStoresCommandHandler(
    IDbConnectionFactory dbConnectionFactory) : ICommandHandler<SyncStoresCommand>
{
    public async Task<Result> Handle(SyncStoresCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            -- UPSERT
            INSERT INTO silver.dim_stores (
                store_id, 
                store_name, 
                phone, 
                email, 
                street, 
                city, 
                state, 
                zip_code
            )
            SELECT
                s.store_id, 
                s.store_name, 
                s.phone, 
                s.email, 
                s.street, 
                s.city, 
                s.state, 
                s.zip_code
            FROM bronze.stores s
            ON CONFLICT (store_id) DO UPDATE SET
                store_name = EXCLUDED.store_name,
                phone = EXCLUDED.phone,
                email = EXCLUDED.email,
                street = EXCLUDED.street,
                city = EXCLUDED.city,
                state = EXCLUDED.state,
                zip_code = EXCLUDED.zip_code;

            -- DELETE stale rows
            DELETE FROM silver.dim_stores ds
            WHERE NOT EXISTS (
                SELECT 1 
                FROM bronze.stores s
                WHERE ds.store_id = s.store_id
            );
            """;

        await connection.ExecuteAsync(sql, cancellationToken);

        return Result.Success();
    }
}
