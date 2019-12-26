using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStoreEF.Data.Mappings
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("City");

            builder.HasKey(c => c.Postalcode);

            builder.Property(c => c.Postalcode)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
