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

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
