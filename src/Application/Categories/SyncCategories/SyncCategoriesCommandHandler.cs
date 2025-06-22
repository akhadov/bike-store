using System.Data.Common;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using SharedKernel;

namespace Application.Categories.SyncCategories;
internal sealed class SyncCategoriesCommandHandler(
    IDbConnectionFactory dbConnectionFactory)
    : ICommandHandler<SyncCategoriesCommand>
{
    public async Task<Result> Handle(SyncCategoriesCommand command, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        return Result.Success();
    }
}
