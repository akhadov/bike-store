using Domain.Silver.DimBrands;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Silver.DimBrands;

internal sealed class DimBrandConfiguration : IEntityTypeConfiguration<DimBrand>
{
    public void Configure(EntityTypeBuilder<DimBrand> builder)
    {
        builder.ToTable("dim_brands", Schemas.Silver);

        builder.HasKey(db => db.BrandId);
    }
}
