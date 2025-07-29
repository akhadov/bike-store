
using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Silver.DimStaffs.SyncStaffs;

internal sealed class SyncStaffsCommandHandler(
    IDbConnectionFactory dbConnectionFactory) : ICommandHandler<SyncStaffsCommand>
{
    public async Task<Result> Handle(SyncStaffsCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
            -- UPSERT
            INSERT INTO silver.dim_staffs (
                staff_id, 
                first_name, 
                last_name, 
                email, 
                phone, 
                active, 
                store_id, 
                manager_id
            )
            SELECT
                s.staff_id, 
                s.first_name, 
                s.last_name, 
                s.email, 
                s.phone, 
                s.active, 
                s.store_id, 
                s.manager_id
            FROM bronze.staffs s
            ON CONFLICT (staff_id) DO UPDATE SET
                first_name = EXCLUDED.first_name,
                last_name = EXCLUDED.last_name,
                email = EXCLUDED.email,
                phone = EXCLUDED.phone,
                active = EXCLUDED.active,
                store_id = EXCLUDED.store_id,
                manager_id = EXCLUDED.manager_id;

            -- DELETE stale rows
            DELETE FROM silver.dim_staffs ds
            WHERE NOT EXISTS (
                SELECT 1 
                FROM bronze.staffs s
                WHERE ds.staff_id = s.staff_id
            );
            """;

        await connection.ExecuteAsync(sql, cancellationToken);

        return Result.Success();
    }
}
