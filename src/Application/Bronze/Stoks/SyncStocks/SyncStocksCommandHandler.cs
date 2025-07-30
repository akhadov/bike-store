using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Bronze.Stocks;
using SharedKernel;

namespace Application.Bronze.Stoks.SyncStocks;

internal sealed class SyncStocksCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncStocksCommand>
{
    public async Task<Result> Handle(SyncStocksCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        List<Stok> stocks = csvService.ReadCsv<Stok>(command.FilePath);

        if (!stocks.Any())
        {
            return Result.Success();
        }

        using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string upsertSql = """
            INSERT INTO bronze.stocks (
                store_id, 
                product_id, 
                quantity
            ) 
            VALUES (
                @StoreId, 
                @ProductId, 
                @Quantity
            )
            ON CONFLICT (store_id, product_id) 
            DO UPDATE SET 
                quantity = EXCLUDED.quantity
            """;

        await connection.ExecuteAsync(upsertSql, stocks, transaction: transaction);

        int[] storeIds = stocks.Select(s => s.StoreId).ToArray();
        int[] productIds = stocks.Select(s => s.ProductId).ToArray();

        const string deleteRemovedSql = """
            DELETE FROM bronze.stocks
            WHERE (store_id, product_id) NOT IN (
                SELECT UNNEST(@StoreIds) AS store_id, UNNEST(@ProductIds) AS product_id
            )
            """;

        await connection.ExecuteAsync(
            deleteRemovedSql,
            new { StoreIds = storeIds, ProductIds = productIds },
            transaction: transaction
        );

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
