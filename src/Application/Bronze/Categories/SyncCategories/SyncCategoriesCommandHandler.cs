using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Bronze.Categories;
using SharedKernel;

namespace Application.Bronze.Categories.SyncCategories;

internal sealed class SyncCategoriesCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncCategoriesCommand>
{
    public async Task<Result> Handle(SyncCategoriesCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        List<Category> categories = csvService.ReadCsv<Category>(command.FilePath);

        if (!categories.Any())
        {
            return Result.Success();
        }

        using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string upsertSql = """
            INSERT INTO bronze.categories (
                category_id, 
                category_name
            ) 
            VALUES (
                @CategoryId, 
                @CategoryName
            )
            ON CONFLICT (category_id) 
            DO UPDATE SET 
                category_name = EXCLUDED.category_name
            """;

        await connection.ExecuteAsync(upsertSql, categories, transaction: transaction);

        int[] categoryIds = categories.Select(b => b.CategoryId).ToArray();

        const string deleteRemovedSql = """
            DELETE FROM bronze.categories 
            WHERE category_id != ALL(@CategoryIds)
            """;

        await connection.ExecuteAsync(deleteRemovedSql, new { CategoryIds = categoryIds }, transaction: transaction);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
