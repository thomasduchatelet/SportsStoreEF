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
                var products = context.Products.ToList();
                products.ForEach(c => Console.WriteLine(c.Name));
            }
        }
    }
}
