using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Dapper;
using Domain.Products;
using SharedKernel;

namespace Application.Products.SyncProducts;

internal sealed class SyncProductsCommandHandler(
    IDbConnectionFactory dbConnectionFactory,
    ICsvService csvService)
    : ICommandHandler<SyncProductsCommand>
{
    public async Task<Result> Handle(SyncProductsCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string createSchemaSql = "CREATE SCHEMA IF NOT EXISTS bronze;";

        await connection.ExecuteAsync(createSchemaSql);

        const string createTableSql = $"""
            CREATE TABLE IF NOT EXISTS bronze.products (
                product_id integer GENERATED BY DEFAULT AS IDENTITY,
                product_name character varying(255) NOT NULL,
                brand_id integer NOT NULL,
                category_id integer NOT NULL,
                model_year integer NOT NULL,
                list_price numeric(10, 2) NOT NULL,
                CONSTRAINT pk_products PRIMARY KEY (product_id)
            );
            """;

        await connection.ExecuteAsync(createTableSql);

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
