using Application.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Application.Bronze.Brands.SyncBramds;
using Application.Bronze.Categories.SyncCategories;
using Application.Bronze.Staffs.SyncStaffs;
using Application.Bronze.Orders.SyncOrderItems;
using Application.Bronze.Orders.SyncOrders;
using Application.Bronze.Products.SyncProducts;
using Application.Bronze.Stores.SyncStores;
using Application.Bronze.Customers.SyncCustomers;
using Application.Bronze.Stoks.SyncStocks;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal sealed class ProcessCsvSyncJob(
    ICommandHandler<SyncBrandsCommand> brandsHandler,
    ICommandHandler<SyncCategoriesCommand> categoriesHandler,
    ICommandHandler<SyncCustomersCommand> customersHandler,
    ICommandHandler<SyncStaffsCommand> staffsHandler,
    ICommandHandler<SyncStoresCommand> storesHandler,
    ICommandHandler<SyncProductsCommand> productsHandler,
    ICommandHandler<SyncOrdersCommand> ordersHandler,
    ICommandHandler<SyncStocksCommand> stocksHandler,
    ICommandHandler<SyncOrderItemsCommand> orderItemsHandler,
    IOptions<CsvSyncJobOptions> csvSyncJobOptions,
    ILogger<ProcessCsvSyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        CancellationToken cancellationToken = context.CancellationToken;

        logger.LogInformation("Beginning to process CSV sync job");

        await brandsHandler.Handle(new SyncBrandsCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "brands.csv")), cancellationToken);

        await categoriesHandler.Handle(new SyncCategoriesCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "categories.csv")), cancellationToken);

        await customersHandler.Handle(new SyncCustomersCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "customers.csv")), cancellationToken);

        await staffsHandler.Handle(new SyncStaffsCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "staffs.csv")), cancellationToken);

        await storesHandler.Handle(new SyncStoresCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "stores.csv")), cancellationToken);

        await productsHandler.Handle(new SyncProductsCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "products.csv")), cancellationToken);

        await ordersHandler.Handle(new SyncOrdersCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "orders.csv")), cancellationToken);

        await stocksHandler.Handle(new SyncStocksCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "stocks.csv")), cancellationToken);

        await orderItemsHandler.Handle(new SyncOrderItemsCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "order_items.csv")), cancellationToken);

        logger.LogInformation("Completed processing CSV sync job");
    }
}
