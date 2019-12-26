using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using SportsStoreEF.Data.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsStoreEF.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionstring = @"Server=.;Database=SportsStore;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionstring);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}
