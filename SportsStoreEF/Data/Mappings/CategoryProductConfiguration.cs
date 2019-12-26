using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStoreEF.Data.Mappings
{
    public class CategoryProductConfiguration : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            builder.ToTable("CategoryProduct");

            builder.HasKey(cp => new { cp.CategoryId, cp.ProductId });

            builder.HasOne(cp => cp.Category)
                .WithMany(cp => cp.Products)
                .HasForeignKey(cp => cp.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(cp => cp.Product)
                .WithMany()
                .HasForeignKey(cp => cp.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
