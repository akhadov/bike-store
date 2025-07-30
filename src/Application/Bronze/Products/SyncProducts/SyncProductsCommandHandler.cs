using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Bronze.Products;
using SharedKernel;

namespace Application.Bronze.Products.SyncProducts;

internal sealed class SyncProductsCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncProductsCommand>
{
    public async Task<Result> Handle(SyncProductsCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        List<Product> products = csvService.ReadCsv<Product>(command.FilePath);

        if (!products.Any())
        {
            return Result.Success();
        }

        using DbTransaction transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string upsertSql = """
            INSERT INTO bronze.products (
                product_id, 
                product_name, 
                brand_id, 
                category_id, 
                model_year, 
                list_price
            ) 
            VALUES (
                @ProductId, 
                @ProductName, 
                @BrandId, 
                @CategoryId, 
                @ModelYear, 
                @ListPrice
            )
            ON CONFLICT (product_id) 
            DO UPDATE SET 
                product_name = EXCLUDED.product_name,
                brand_id = EXCLUDED.brand_id,
                category_id = EXCLUDED.category_id,
                model_year = EXCLUDED.model_year,
                list_price = EXCLUDED.list_price
            """;

        await connection.ExecuteAsync(upsertSql, products, transaction: transaction);

        int[] productIds = products.Select(b => b.ProductId).ToArray();

        const string deleteRemovedSql = """
            DELETE FROM bronze.products 
            WHERE product_id != ALL(@ProductIds)
            """;

        await connection.ExecuteAsync(deleteRemovedSql, new { ProductIds = productIds }, transaction: transaction);

        await transaction.CommitAsync(cancellationToken);

        return Result.Success();
    }
}
