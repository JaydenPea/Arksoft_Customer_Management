using Arksoft.CustomerManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Arksoft.CustomerManagement.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.TelephoneNumber)
            .HasMaxLength(20);

        builder.Property(c => c.ContactPersonName)
            .HasMaxLength(100);

        builder.Property(c => c.ContactPersonEmail)
            .HasMaxLength(255);

        builder.Property(c => c.VatNumber)
            .HasMaxLength(50);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired(false);

        // Indexes for performance
        builder.HasIndex(c => c.Name);
        builder.HasIndex(c => c.VatNumber);
        builder.HasIndex(c => c.ContactPersonEmail);
    }
}