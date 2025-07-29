using Domain.Silver.DimCategories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Silver.DimCategories;

internal sealed class DimCategoryConfiguration : IEntityTypeConfiguration<DimCategory>
{
    public void Configure(EntityTypeBuilder<DimCategory> builder)
    {
        builder.ToTable("dim_categories", Schemas.Silver);

        builder.HasKey(dc => dc.CategoryId);
    }
}
