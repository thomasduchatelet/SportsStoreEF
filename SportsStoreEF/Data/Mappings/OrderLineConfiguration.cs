using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStoreEF.Data.Mappings
{
    public class OrderLineConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.ToTable("OrderLine");

            builder.HasKey(ol => new { ol.OrderId, ol.ProductId });

            builder.HasOne(ol => ol.Product).WithMany().HasForeignKey(ol => ol.ProductId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
