using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Bronze.Orders;
using SharedKernel;

namespace Application.Bronze.Orders.SyncOrderItems;

internal sealed class SyncOrderItemsCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncOrderItemsCommand>
{
    public async Task<Result> Handle(SyncOrderItemsCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        List<OrderItem> orderItems = csvService.ReadCsv<OrderItem>(command.FilePath);

        if (!orderItems.Any())
        {
            return Result.Success();
        }

        using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string upsertSql = """
            INSERT INTO bronze.order_items (
                item_id, 
                order_id, 
                product_id, 
                quantity, 
                list_price, 
                discount
            ) 
            VALUES (
                @ItemId, 
                @OrderId, 
                @ProductId, 
                @Quantity, 
                @ListPrice, 
                @Discount
            )
            ON CONFLICT (item_id) 
            DO UPDATE SET 
                order_id = EXCLUDED.order_id,
                product_id = EXCLUDED.product_id,
                quantity = EXCLUDED.quantity,
                list_price = EXCLUDED.list_price,
                discount = EXCLUDED.discount
            """;

        await connection.ExecuteAsync(upsertSql, orderItems, transaction: transaction);

        int[] itemIds = orderItems.Select(b => b.ItemId).ToArray();

        const string deleteRemovedSql = """
            DELETE FROM bronze.order_items 
            WHERE item_id != ALL(@ItemIds)
            """;

        await connection.ExecuteAsync(deleteRemovedSql, new { ItemIds = itemIds }, transaction: transaction);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
