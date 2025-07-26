using Domain.Customers;
using Domain.Orders;
using Domain.Staffs;
using Domain.Stores;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Orders;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", Schemas.Bronze);

        builder.HasKey(o => o.OrderId);

        builder.HasOne<Customer>().WithMany().HasForeignKey(o => o.CustomerId);

        builder.HasMany(o => o.OrderItems).WithOne().HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Store>().WithMany().HasForeignKey(o => o.StoreId);

        builder.HasOne<Staff>().WithMany().HasForeignKey(o => o.StaffId);
    }
}
