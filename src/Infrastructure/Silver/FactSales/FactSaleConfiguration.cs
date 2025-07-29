using Domain.Silver.FactSales;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Silver.FactSales;

internal sealed class FactSaleConfiguration : IEntityTypeConfiguration<FactSale>
{
    public void Configure(EntityTypeBuilder<FactSale> builder)
    {
        builder.ToTable("fact_sales", Schemas.Silver);

        builder.HasKey(fs => new { fs.OrderId, fs.ItemId });

        builder.Property(fs => fs.TotalPrice)
               .HasComputedColumnSql("quantity * list_price * (1 - discount)", stored: true);
    }
}
