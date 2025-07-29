using Domain.Silver.DimCustomers;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Silver.DimCustomers;

internal sealed class DimCustomerConfiguration : IEntityTypeConfiguration<DimCustomer>
{
    public void Configure(EntityTypeBuilder<DimCustomer> builder)
    {
        builder.ToTable("dim_customers", Schemas.Silver);

        builder.HasKey(dc => dc.CustomerId);
    }
}
