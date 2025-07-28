using Domain.Bronze.Products;
using Domain.Bronze.Stocks;
using Domain.Bronze.Stores;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Bronze.Stocks;

internal sealed class StockConfiguration : IEntityTypeConfiguration<Stok>
{
    public void Configure(EntityTypeBuilder<Stok> builder)
    {
        builder.ToTable("stocks", Schemas.Bronze);

        builder.HasKey(s => new { s.StoreId, s.ProductId });

        builder.Property(s => s.Quantity).IsRequired().HasDefaultValue(0);

        builder.HasOne<Store>()
            .WithMany()
            .HasForeignKey(s => s.StoreId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
