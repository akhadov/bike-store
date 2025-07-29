
using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Silver.DimCustomers.SyncCustomers;

internal sealed class SyncCustomersCommandHandler(
    IDbConnectionFactory dbConnectionFactory) : ICommandHandler<SyncCustomersCommand>
{
    public async Task<Result> Handle(SyncCustomersCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            -- UPSERT
            INSERT INTO silver.dim_customers (
                customer_id, 
                first_name, 
                last_name, 
                phone, 
                email, 
                street, 
                city, 
                state, 
                zip_code
            )
            SELECT
                c.customer_id, 
                c.first_name, 
                c.last_name, 
                c.phone, 
                c.email, 
                c.street, 
                c.city, 
                c.state, 
                c.zip_code
            FROM bronze.customers c
            ON CONFLICT (customer_id) DO UPDATE SET
                first_name = EXCLUDED.first_name,
                last_name = EXCLUDED.last_name,
                phone = EXCLUDED.phone,
                email = EXCLUDED.email,
                street = EXCLUDED.street,
                city = EXCLUDED.city,
                state = EXCLUDED.state,
                zip_code = EXCLUDED.zip_code;

            -- DELETE stale rows
            DELETE FROM silver.dim_customers dc
            WHERE NOT EXISTS (
                SELECT 1 
                FROM bronze.customers c
                WHERE dc.customer_id = c.customer_id
            );
            """;

        await connection.ExecuteAsync(sql, cancellationToken);

        return Result.Success();
    }
}
