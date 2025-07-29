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
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    // Bronze
    DbSet<Brand> Brands { get; }
    DbSet<Category> Categories { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Product> Products { get; }
    DbSet<Staff> Staffs { get; }
    DbSet<Stok> Stoks { get; }
    DbSet<Store> Stores { get; }
    DbSet<User> Users { get; }
    DbSet<TodoItem> TodoItems { get; }

    // Silver
    DbSet<DimBrand> DimBrands { get; }
    DbSet<DimCategory> DimCategories { get; }
    DbSet<DimCustomer> DimCustomers { get; }
    DbSet<DimProduct> DimProducts { get; }
    DbSet<DimStaff> DimStaffs { get; }
    DbSet<DimStore> DimStores { get; }
    DbSet<FactInventory> FactInventory { get; }
    DbSet<FactSale> FactSales { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
