using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStoreEF.Data
{
    public class SportsStoreDataInitializer
    {
        private readonly ApplicationDbContext _context;

        public SportsStoreDataInitializer(ApplicationDbContext context)
        {
            _context = context;
        }
        public void InitializeRecords()
        {
            if (!_context.Products.Any())
            {


                Product football = new Product("Football", 25, "WK colors");
                OnlineProduct cornerflags = new OnlineProduct("Corner flags", 34.95M, "cornerflags_thumb.png", "Give your playing field that professional touch");
                Product shoes = new Product("Running shoes", 95, "Protective and fashionable");
                Product surfboard = new Product("Surf board", 275, "A boat for one person");
                Product kayak = new Product("Kayak", 170, "High quality");
                Product lifeJacket = new Product("Lifejacket", 49.99M, "Protective and fashionable");

                Category watersports = new Category("WaterSports");
                Category soccer = new Category("Soccer");
                soccer.AddProduct(football);
                soccer.AddProduct(cornerflags);
                soccer.AddProduct(shoes);
                watersports.AddProduct(shoes);
                watersports.AddProduct(surfboard);
                watersports.AddProduct(kayak);
                watersports.AddProduct(lifeJacket);
                _context.AddRange(new Category[] { watersports, soccer });

                City gent = new City("9000", "Gent");
                City antwerpen = new City("3000", "Antwerpen");
                City[] cities = { gent, antwerpen };
                _context.AddRange(cities);

                _context.SaveChanges();
                Random r = new Random();
                for (int i = 1; i < 10; i++)
                {
                    Customer klant = new Customer("student" + i, "Student" + i, "Jan", "Nieuwstraat 10", cities[r.Next(2)]);
                    if (i <= 5)
                    {
                        Cart cart = new Cart();
                        cart.AddLine(football, 1);
                        cart.AddLine(cornerflags, 2);
                        klant.PlaceOrder(cart, DateTime.Today, false, klant.Street, klant.City);
                    }
                    _context.Add(klant);
                }

                _context.SaveChanges();
            }
        }
    }
}
