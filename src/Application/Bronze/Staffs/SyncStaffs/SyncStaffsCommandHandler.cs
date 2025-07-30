using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Bronze.Staffs;
using SharedKernel;

namespace Application.Bronze.Staffs.SyncStaffs;

internal sealed class SyncStaffsCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncStaffsCommand>
{
    public async Task<Result> Handle(SyncStaffsCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        List<Staff> staffs = csvService.ReadCsv<Staff>(command.FilePath);

        if (!staffs.Any())
        {
            return Result.Success();
        }

        using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string upsertSql = """
            INSERT INTO bronze.staffs (
                staff_id, 
                first_name, 
                last_name, 
                email, 
                phone, 
                active, 
                store_id, 
                manager_id
            ) 
            VALUES (
                @StaffId, 
                @FirstName, 
                @LastName, 
                @Email, 
                @Phone, 
                @Active, 
                @StoreId, 
                @ManagerId
            )
            ON CONFLICT (staff_id) 
            DO UPDATE SET 
                first_name = EXCLUDED.first_name,
                last_name = EXCLUDED.last_name,
                email = EXCLUDED.email,
                phone = EXCLUDED.phone,
                active = EXCLUDED.active,
                store_id = EXCLUDED.store_id,
                manager_id = EXCLUDED.manager_id
            """;

        await connection.ExecuteAsync(upsertSql, staffs, transaction: transaction);

        int[] staffIds = staffs.Select(b => b.StaffId).ToArray();

        const string deleteRemovedSql = """
            DELETE FROM bronze.staffs 
            WHERE staff_id != ALL(@StaffIds)
            """;

        await connection.ExecuteAsync(deleteRemovedSql, new { StaffIds = staffIds }, transaction: transaction);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
