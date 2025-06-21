using Domain.Brands;
using Domain.Categories;
using Domain.Products;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Products;
internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products", Schemas.Silver);

        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.ProductName).HasMaxLength(500).IsRequired();

        builder.HasOne<Brand>().WithMany().HasForeignKey(p => p.BrandId);

        builder.HasOne<Category>().WithMany().HasForeignKey(c => c.CategoryId);
    }
}
