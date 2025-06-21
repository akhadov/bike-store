using Domain.Customers;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Customers;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers", Schemas.Silver);

        builder.HasKey(c => c.CustomerId);

        builder.Property(c => c.FirstName).HasMaxLength(250);

        builder.Property(c => c.LastName).HasMaxLength(250);

        builder.Property(c => c.Phone).HasMaxLength(50);

        builder.Property(c => c.Email).HasMaxLength(500);

        builder.HasIndex(c => c.Email).IsUnique();
    }
}
