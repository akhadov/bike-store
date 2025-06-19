using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Categories.SyncCategoriesFromCsv;
internal sealed class SyncCategoriesFromCsvCommandHandler(
    IDbConnectionFactory dbConnectionFactory)
    : ICommandHandler<SyncCategoriesFromCsvCommand>
{
    public async Task<Result> Handle(SyncCategoriesFromCsvCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string categoriesSql =
            $"""
             SELECT 
                category_id AS {nameof(SyncCategoriesResponse.CategoryId)},
                category_name AS {nameof(SyncCategoriesResponse.CategoryName)}
             FROM bronze.categories
             ORDER BY Id
            """;

        List<SyncCategoriesResponse> categories = (await connection.QueryAsync<SyncCategoriesResponse>(categoriesSql)).AsList();

        return Result.Success();
    }
}
