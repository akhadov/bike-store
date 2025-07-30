using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Bronze.Stores;
using SharedKernel;

namespace Application.Bronze.Stores.SyncStores;

internal sealed class SyncStoresCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncStoresCommand>
{
    public async Task<Result> Handle(SyncStoresCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        List<Store> stores = csvService.ReadCsv<Store>(command.FilePath);

        if (!stores.Any())
        {
            return Result.Success();
        }

        using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string upsertSql = """
            INSERT INTO bronze.stores (
                store_id, 
                store_name, 
                phone, 
                email, 
                street, 
                city, 
                state, 
                zip_code
            ) 
            VALUES (
                @StoreId, 
                @StoreName, 
                @Phone, 
                @Email, 
                @Street, 
                @City, 
                @State, 
                @ZipCode
            )
            ON CONFLICT (store_id) 
            DO UPDATE SET 
                store_name = EXCLUDED.store_name,
                phone = EXCLUDED.phone,
                email = EXCLUDED.email,
                street = EXCLUDED.street,
                city = EXCLUDED.city,
                state = EXCLUDED.state,
                zip_code = EXCLUDED.zip_code
            """;

        await connection.ExecuteAsync(upsertSql, stores, transaction: transaction);

        int[] storeIds = stores.Select(b => b.StoreId).ToArray();

        const string deleteRemovedSql = """
            DELETE FROM bronze.stores 
            WHERE store_id != ALL(@StoreIds)
            """;

        await connection.ExecuteAsync(deleteRemovedSql, new { StoreIds = storeIds }, transaction: transaction);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
