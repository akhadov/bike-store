using Domain.Silver.DimProducts;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Silver.DimProducts;

internal sealed class DimProductConfiguration : IEntityTypeConfiguration<DimProduct>
{
    public void Configure(EntityTypeBuilder<DimProduct> builder)
    {
        builder.ToTable("dim_products", Schemas.Silver);

        builder.HasKey(dp => dp.ProductId);
    }
}
