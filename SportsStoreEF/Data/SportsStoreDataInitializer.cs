﻿using SportsStore.Models;
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
                Product cornerflags = new Product("Corner flags", 34.95M, "Give your playing field that professional touch");
                Product shoes = new Product("Running shoes", 95, "Protective and fashionable");
                Product surfboard = new Product("Surf board", 275, "A boat for one person");
                Product kayak = new Product("Kayak", 170, "High quality");
                Product lifeJacket = new Product("Lifejacket", 49.99M, "Protective and fashionable");
                Product[] products = { football, cornerflags, shoes, surfboard, kayak, lifeJacket };
                _context.AddRange(products);

                City gent = new City("9000", "Gent");
                City antwerpen = new City("3000", "Antwerpen");
                City[] cities = { gent, antwerpen };
                _context.AddRange(cities);

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
                }

                _context.SaveChanges();
            }
        }
    }
}