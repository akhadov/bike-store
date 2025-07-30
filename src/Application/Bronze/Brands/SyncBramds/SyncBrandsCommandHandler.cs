using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Bronze.Brands;
using SharedKernel;

namespace Application.Bronze.Brands.SyncBramds;

internal sealed class SyncBrandsCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncBrandsCommand>
{
    public async Task<Result> Handle(SyncBrandsCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        List<Brand> brands = csvService.ReadCsv<Brand>(command.FilePath);

        if (!brands.Any())
        {
            return Result.Success();
        }

        using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string upsertSql = """
            INSERT INTO bronze.brands (
                brand_id, 
                brand_name
            ) 
            VALUES (
                @BrandId, 
                @BrandName
            )
            ON CONFLICT (brand_id) 
            DO UPDATE SET 
                brand_name = EXCLUDED.brand_name
            """;

        await connection.ExecuteAsync(upsertSql, brands, transaction: transaction);

        int[] brandIds = brands.Select(b => b.BrandId).ToArray();

        const string deleteRemovedSql = """
            DELETE FROM bronze.brands 
            WHERE brand_id != ALL(@BrandIds)
            """;

        await connection.ExecuteAsync(deleteRemovedSql, new { BrandIds = brandIds }, transaction: transaction);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
