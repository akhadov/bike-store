using Domain.Orders;
using Domain.Products;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Orders;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items", Schemas.Bronze);

        builder.HasKey(c => c.ItemId);

        //builder.Property(c => c.ItemId).ValueGeneratedNever();
        //builder.Property(oi => oi.ItemId).ValueGeneratedOnAdd(); // Auto-increment

        builder.HasOne<Product>().WithMany().HasForeignKey(oi => oi.ProductId);
    }
}
