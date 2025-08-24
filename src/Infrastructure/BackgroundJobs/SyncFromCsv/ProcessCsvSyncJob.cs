using Application.Abstractions.Messaging;
using Application.Bronze.Brands.SyncBramds;
using Application.Bronze.Categories.SyncCategories;
using Application.Bronze.Customers.SyncCustomers;
using Application.Bronze.Orders.SyncOrderItems;
using Application.Bronze.Orders.SyncOrders;
using Application.Bronze.Products.SyncProducts;
using Application.Bronze.Staffs.SyncStaffs;
using Application.Bronze.Stoks.SyncStocks;
using Application.Bronze.Stores.SyncStores;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.BackgroundJobs.SyncFromCsv;

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
        logger.LogInformation("Beginning to process CSV sync job");

        await brandsHandler.Handle(new SyncBrandsCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "brands.csv")), context.CancellationToken);

        await categoriesHandler.Handle(new SyncCategoriesCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "categories.csv")), context.CancellationToken);

        await customersHandler.Handle(new SyncCustomersCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "customers.csv")), context.CancellationToken);

        await staffsHandler.Handle(new SyncStaffsCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "staffs.csv")), context.CancellationToken);

        await storesHandler.Handle(new SyncStoresCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "stores.csv")), context.CancellationToken);

        await productsHandler.Handle(new SyncProductsCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "products.csv")), context.CancellationToken);

        await ordersHandler.Handle(new SyncOrdersCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "orders.csv")), context.CancellationToken);

        await stocksHandler.Handle(new SyncStocksCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "stocks.csv")), context.CancellationToken);

        await orderItemsHandler.Handle(new SyncOrderItemsCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "order_items.csv")), context.CancellationToken);

        logger.LogInformation("Completed processing CSV sync job");
    }
}
