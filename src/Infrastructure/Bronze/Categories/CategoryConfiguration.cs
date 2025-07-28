using Domain.Bronze.Categories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Bronze.Categories;

internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories", Schemas.Bronze);

        builder.HasKey(c => c.CategoryId);

        builder.Property(c => c.CategoryName).HasMaxLength(500);
    }
}
