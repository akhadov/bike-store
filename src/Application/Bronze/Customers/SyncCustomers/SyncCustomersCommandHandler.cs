using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Bronze.Customers;
using SharedKernel;

namespace Application.Bronze.Customers.SyncCustomers;

internal sealed class SyncCustomersCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncCustomersCommand>
{
    public async Task<Result> Handle(SyncCustomersCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        List<Customer> customers = csvService.ReadCsv<Customer>(command.FilePath);

        if (!customers.Any())
        {
            return Result.Success();
        }

        using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string upsertSql = """
            INSERT INTO bronze.customers (
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
            VALUES (
                @CustomerId, 
                @FirstName, 
                @LastName, 
                @Phone, 
                @Email, 
                @Street, 
                @City, 
                @State, 
                @ZipCode
            )
            ON CONFLICT (customer_id) 
            DO UPDATE SET 
                first_name = EXCLUDED.first_name,
                last_name = EXCLUDED.last_name,
                phone = EXCLUDED.phone,
                email = EXCLUDED.email,
                street = EXCLUDED.street,
                city = EXCLUDED.city,
                state = EXCLUDED.state,
                zip_code = EXCLUDED.zip_code
            """;

        await connection.ExecuteAsync(upsertSql, customers, transaction: transaction);

        int[] customerIds = customers.Select(b => b.CustomerId).ToArray();

        const string deleteRemovedSql = """
            DELETE FROM bronze.customers 
            WHERE customer_id != ALL(@CustomerIds)
            """;

        await connection.ExecuteAsync(deleteRemovedSql, new { CustomerIds = customerIds }, transaction: transaction);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
