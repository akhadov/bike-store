using Domain.Bronze.Brands;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Bronze.Brands;

internal sealed class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("brands", Schemas.Bronze);

        builder.HasKey(b => b.BrandId);

        builder.Property(b => b.BrandName).HasMaxLength(500);
    }
}
