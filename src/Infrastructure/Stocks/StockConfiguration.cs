using Domain.Products;
using Domain.Stocks;
using Domain.Stores;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Stocks;

internal sealed class StockConfiguration : IEntityTypeConfiguration<Stok>
{
    public void Configure(EntityTypeBuilder<Stok> builder)
    {
        builder.ToTable("stocks", Schemas.Silver);

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
