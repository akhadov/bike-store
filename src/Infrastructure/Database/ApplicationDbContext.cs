using System.Data;
using Application.Abstractions.Data;
using Domain.Bronze.Brands;
using Domain.Bronze.Categories;
using Domain.Bronze.Customers;
using Domain.Bronze.Orders;
using Domain.Bronze.Products;
using Domain.Bronze.Staffs;
using Domain.Bronze.Stocks;
using Domain.Bronze.Stores;
using Domain.Bronze.Todos;
using Domain.Bronze.Users;
using Domain.Silver.DimBrands;
using Domain.Silver.DimCategories;
using Domain.Silver.DimCustomers;
using Domain.Silver.DimProducts;
using Domain.Silver.DimStaffs;
using Domain.Silver.DimStores;
using Domain.Silver.FactInventories;
using Domain.Silver.FactSales;
using Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel;

namespace Infrastructure.Database;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IDomainEventsDispatcher domainEventsDispatcher)
    : DbContext(options), IApplicationDbContext
{
    // Bronze
    public DbSet<User> Users { get; set; }

    public DbSet<TodoItem> TodoItems { get; set; }

    public DbSet<Brand> Brands { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Staff> Staffs { get; set; }

    public DbSet<Stok> Stoks { get; set; }

    public DbSet<Store> Stores { get; set; }

    public DbSet<Customer> Customers { get; set; }

    // Silver
    public DbSet<DimBrand> DimBrands { get; set; }

    public DbSet<DimCategory> DimCategories { get; set; }

    public DbSet<DimCustomer> DimCustomers { get; set; }

    public DbSet<DimProduct> DimProducts { get; set; }

    public DbSet<DimStaff> DimStaffs { get; set; }

    public DbSet<DimStore> DimStores { get; set; }

    public DbSet<FactInventory> FactInventory { get; set; }

    public DbSet<FactSale> FactSales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
        modelBuilder.HasDefaultSchema(Schemas.Bronze);
        modelBuilder.HasDefaultSchema(Schemas.Silver);
        modelBuilder.HasDefaultSchema(Schemas.Gold);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // When should you publish domain events?
        //
        // 1. BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // 2. AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail

        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        await domainEventsDispatcher.DispatchAsync(domainEvents);
    }
    public async Task<IDbTransaction> BeginTransactionAsync()
    {
        return (await Database.BeginTransactionAsync()).GetDbTransaction();
    }
}
