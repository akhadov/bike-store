using Domain.Bronze.Brands;
using Domain.Bronze.Categories;
using Domain.Bronze.Products;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Bronze.Products;
internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products", Schemas.Bronze);

        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.ProductName).HasMaxLength(500).IsRequired();

        builder.HasOne<Brand>().WithMany().HasForeignKey(p => p.BrandId);

        builder.HasOne<Category>().WithMany().HasForeignKey(c => c.CategoryId);
    }
}
