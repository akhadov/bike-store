using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Bronze.Orders;
using SharedKernel;

namespace Application.Bronze.Orders.SyncOrders;

internal sealed class SyncOrdersCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncOrdersCommand>
{
    public async Task<Result> Handle(SyncOrdersCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        List<Order> orders = csvService.ReadCsv<Order>(command.FilePath);

        if (!orders.Any())
        {
            return Result.Success();
        }

        using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string upsertSql = """
            INSERT INTO bronze.orders (
                order_id, 
                customer_id, 
                order_status, 
                order_date, 
                required_date, 
                shipped_date, 
                store_id, 
                staff_id
            ) 
            VALUES (
                @OrderId, 
                @CustomerId, 
                @OrderStatus, 
                @OrderDate, 
                @RequiredDate, 
                @ShippedDate, 
                @StoreId, 
                @StaffId
            )
            ON CONFLICT (order_id) 
            DO UPDATE SET 
                customer_id = EXCLUDED.customer_id,
                order_status = EXCLUDED.order_status,
                order_date = EXCLUDED.order_date,
                required_date = EXCLUDED.required_date,
                shipped_date = EXCLUDED.shipped_date,
                store_id = EXCLUDED.store_id,
                staff_id = EXCLUDED.staff_id
            """;

        await connection.ExecuteAsync(upsertSql, orders, transaction: transaction);

        int[] orderIds = orders.Select(b => b.OrderId).ToArray();

        const string deleteRemovedSql = """
            DELETE FROM bronze.orders 
            WHERE order_id != ALL(@OrderIds)
            """;

        await connection.ExecuteAsync(deleteRemovedSql, new { OrderIds = orderIds }, transaction: transaction);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
