using Domain.Brands;
using Domain.Categories;
using Domain.Customers;
using Domain.Orders;
using Domain.Products;
using Domain.Staffs;
using Domain.Stocks;
using Domain.Stores;
using Domain.Todos;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
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

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
