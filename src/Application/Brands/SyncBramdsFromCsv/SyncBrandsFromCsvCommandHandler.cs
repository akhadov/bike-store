using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Brands.SyncBramdsFromCsv;
internal sealed class SyncBrandsFromCsvCommandHandler(
    //IDbConnectionFactory dbConnectionFactory
    )
    : ICommandHandler<SyncBrandsFromCsvCommand>
{
    public Task<Result> Handle(SyncBrandsFromCsvCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
