using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStoreEF.Data.Mappings
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            builder.Property(c => c.CustomerName)
                .IsRequired()
                .HasMaxLength(20);
            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(c => c.Street)
                .HasMaxLength(100);

            builder.HasOne(c => c.City)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Orders).WithOne().IsRequired().OnDelete(DeleteBehavior.Cascade);
                
        }
    }
}
