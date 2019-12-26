using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStoreEF.Data.Mappings
{
    public class OnlineProductConfiguration : IEntityTypeConfiguration<OnlineProduct>
    {
        public void Configure(EntityTypeBuilder<OnlineProduct> builder)
        {
            //builder.HasBaseType<Product>();
            builder.Property(op => op.ThumbNail)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
