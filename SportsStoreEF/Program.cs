using SportsStoreEF.Data;
using System;
using System.Linq;

namespace SportsStoreEF
{
    class Program
    {
        static void Main(string[] args)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Database.EnsureDeleted();
                Console.WriteLine("Database deleted");
                context.Database.EnsureCreated();
                Console.WriteLine("Database created");

                SportsStoreDataInitializer initializer = new SportsStoreDataInitializer(context);
                initializer.InitializeRecords();
                Console.WriteLine("Records initialized");
                var Categories = context.Categories.ToList();
                Categories.ForEach(c =>
                {
                    Console.WriteLine($"Products of category {c.Name}:");
                    c.Products.ToList().ForEach(p => Console.WriteLine($"{p.Product.Name}: {p.Product.Description}"));
                });
            }
        }
    }
}
