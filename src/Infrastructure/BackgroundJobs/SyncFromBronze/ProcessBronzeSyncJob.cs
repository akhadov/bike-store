using Application.Abstractions.Messaging;
using Application.Silver.DimBrands.SyncBrands;
using Application.Silver.DimCategories.SyncCategories;
using Application.Silver.DimCustomers.SyncCustomers;
using Application.Silver.DimProducts.SyncProducts;
using Application.Silver.DimStaffs.SyncStaffs;
using Application.Silver.DimStores.SyncStores;
using Application.Silver.FactInventories.SyncInventories;
using Application.Silver.FactSales.SyncSales;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.BackgroundJobs.SyncFromBronze;

[DisallowConcurrentExecution]
internal sealed class ProcessBronzeSyncJob(
    ICommandHandler<SyncSalesCommand> salesHandler,
    ICommandHandler<SyncInventoriesCommand> inventoriesHandler,
    ICommandHandler<SyncBrandsCommand> brandsHandler,
    ICommandHandler<SyncCategoriesCommand> categoriesHandler,
    ICommandHandler<SyncCustomersCommand> customersHandler,
    ICommandHandler<SyncProductsCommand> productsHandler,
    ICommandHandler<SyncStaffsCommand> staffsHandler,
    ICommandHandler<SyncStoresCommand> storesHandler,
    ILogger<ProcessBronzeSyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning to process Bronze sync job");

        await salesHandler.Handle(new SyncSalesCommand(), context.CancellationToken);

        await inventoriesHandler.Handle(new SyncInventoriesCommand(), context.CancellationToken);

        await brandsHandler.Handle(new SyncBrandsCommand(), context.CancellationToken);

        await categoriesHandler.Handle(new SyncCategoriesCommand(), context.CancellationToken);

        await customersHandler.Handle(new SyncCustomersCommand(), context.CancellationToken);

        await staffsHandler.Handle(new SyncStaffsCommand(), context.CancellationToken);

        await storesHandler.Handle(new SyncStoresCommand(), context.CancellationToken);

        await productsHandler.Handle(new SyncProductsCommand(), context.CancellationToken);

        logger.LogInformation("ProcessBronzeSyncJob completed successfully");
    }
}
