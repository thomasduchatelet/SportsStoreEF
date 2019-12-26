using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStoreEF.Data.Mappings
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.Property(o => o.ShippingStreet)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(o => o.OrderLines).WithOne().IsRequired().HasForeignKey(t => t.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.ShippingCity).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
