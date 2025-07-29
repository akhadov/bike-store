using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Silver.FactSales.SyncSales;

internal sealed class SyncSalesCommandHandler(
    IDbConnectionFactory dbConnectionFactory) : ICommandHandler<SyncSalesCommand>
{
    public async Task<Result> Handle(SyncSalesCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
                -- UPSERT
            INSERT INTO silver.fact_sales (
                order_id,
                item_id,
                order_date, 
                shipped_date, 
                store_id,
                staff_id,
                customer_id,
                product_id,
                quantity,
                list_price,
                discount
            )
            SELECT
                o.order_id, 
                oi.item_id,
                o.order_date, 
                o.shipped_date, 
                o.store_id,
                o.staff_id,
                o.customer_id,
                oi.product_id,
                oi.quantity,
                oi.list_price,
                oi.discount
            FROM bronze.orders o
            JOIN bronze.order_items oi ON o.order_id = oi.order_id
            ON CONFLICT (order_id, item_id) DO UPDATE SET
                order_date = EXCLUDED.order_date,
                shipped_date = EXCLUDED.shipped_date,
                store_id = EXCLUDED.store_id,
                staff_id = EXCLUDED.staff_id,
                customer_id = EXCLUDED.customer_id,
                product_id = EXCLUDED.product_id,
                quantity = EXCLUDED.quantity,
                list_price = EXCLUDED.list_price,
                discount = EXCLUDED.discount;

            -- DELETE stale rows
            DELETE FROM silver.fact_sales fs
            WHERE NOT EXISTS (
                SELECT 1 
                FROM bronze.orders o 
                JOIN bronze.order_items oi ON o.order_id = oi.order_id
                WHERE fs.order_id = o.order_id AND fs.item_id = oi.item_id
            );
            """;

        await connection.ExecuteAsync(sql, cancellationToken);

        return Result.Success();
    }
}
