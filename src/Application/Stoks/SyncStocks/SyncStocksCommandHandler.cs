using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Stocks;
using SharedKernel;

namespace Application.Stoks.SyncStocks;

internal sealed class SyncStocksCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncStocksCommand>
{
    public async Task<Result> Handle(SyncStocksCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string createSchemaSql = "CREATE SCHEMA IF NOT EXISTS bronze;";

        await connection.ExecuteAsync(createSchemaSql);

        const string createTableSql = $"""
            CREATE TABLE IF NOT EXISTS bronze.stocks (
                store_id integer NOT NULL,
                product_id integer NOT NULL,
                quantity integer NOT NULL,
                CONSTRAINT pk_stocks PRIMARY KEY (store_id, product_id)
            );
            """;

        await connection.ExecuteAsync(createTableSql);

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

        const string deleteRemovedSql = """
            DELETE FROM bronze.stocks
            WHERE (store_id, product_id) NOT IN (SELECT store_id, product_id FROM UNNEST(@StockIds) AS s(store_id integer, product_id integer))
            """;

        var stockIds = stocks.Select(s => new { s.StoreId, s.ProductId }).ToArray();

        await connection.ExecuteAsync(deleteRemovedSql, new { StockIds = stockIds }, transaction: transaction);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
