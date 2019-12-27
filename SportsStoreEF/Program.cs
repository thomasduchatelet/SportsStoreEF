using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using SportsStoreEF.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsStoreEF
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Database.EnsureDeleted();
                Console.WriteLine("Database deleted");
                context.Database.EnsureCreated();
                Console.WriteLine("Database created");

                SportsStoreDataInitializer initializer = new SportsStoreDataInitializer(context);
                initializer.InitializeRecords();
                Console.WriteLine("Records initialized");
                var Categories = context.Categories.Include(c => c.Products).ThenInclude(cp => cp.Product).ToList();
                Categories.ForEach(c =>
                {
                    Console.WriteLine($"Products of category {c.Name}:");
                    c.Products.ToList().ForEach(p => Console.WriteLine($"{p.Product.Name}: {p.Product.Description}"));
                });

                DoExercise();
            }
        }
        private static void DoExercise()
        {

            IEnumerable<Product> products;
            Product product;
            IEnumerable<Customer> customers;
            Customer customer = null;


            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("--1. Alle producten gesorteerd op prijs oplopend, dan op naam--");
                products = context.Products.OrderBy(p => p.Price).ThenBy(p => p.Name);
                WriteProducts(products);
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--2. Alle klanten die wonen te gent, gesorteerd op naam --");
                customers = context.Customers.Where(c => c.City.Name.ToUpper().Equals("GENT")).OrderBy(c => c.Name).ToList();
                WriteCustomers(customers);
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--3. Het aantal klanten in Gent--");
                int count = context.Customers.Where(c => c.City.Name == "Gent").Count();
                Console.WriteLine(count);
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--4. De klant met  customername student4. --");
                try
                {
                    customer = context.Customers.Include(c => c.Orders).ThenInclude(o => o.OrderLines).ThenInclude(ol => ol.Product).SingleOrDefault(c => c.CustomerName.ToUpper().Equals("STUDENT4"));
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Er zijn meerdere klanten met deze customername");
                }
                Console.WriteLine(customer is null ? "Er zijn geen klanten met deze customername" : customer.Name + " " + customer.FirstName);

                Console.WriteLine("\n--5. Vervolg op vorige query. Print nu de orders van die klant af. Print (methode WriteOrder) de Orderdate, DeliveryDate, en Total af. --");
                //Bekijk de Property Total in de klasse Order. Haal de klant niet opnieuw op! Haal klant en zijn Orders en Orderlines in 1 keer op. 
                //zie methode WriteOrder voor het weergeven van een order
                if (customer != null)
                    foreach (Order o in customer.Orders)
                        WriteOrder(o);
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--6. Alle producten van de categorie soccer, gesorteerd op prijs descending--");
                products = context.Categories.Include(c => c.Products).ThenInclude(cp => cp.Product).SingleOrDefault(c => c.Name.ToUpper().Equals("SOCCER")).Products.Select(cp => cp.Product).OrderByDescending(p => p.Price);
                WriteProducts(products);
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--7A. Maak een nieuwe cart aan en voeg product met id 1 toe(aantal 2).--");
                Cart cart = new Cart();
                product = context.Products.FirstOrDefault(p => p.ProductId == 1);
                cart.AddLine(product, 2);


                Console.WriteLine("\n--7B. Plaatst dan een order voor student 4 voor deze cart, deliverydate binnen 20 dagen, giftwrapping false en deliveryAddress = adres van de klant--. Persisteer in database. Print vervolgens alle orders van de klant.");

                if (customer != null)
                {
                    customer = context.Customers
                        .Include(c => c.Orders).ThenInclude(o => o.OrderLines).ThenInclude(ol => ol.Product).Include(c => c.City)
                        .FirstOrDefault(c => c.CustomerName == "student4");
                    customer.PlaceOrder(cart, DateTime.Now.AddDays(20), false, customer.Street, customer.City);
                    context.SaveChanges();
                    foreach (Order o in customer.Orders)
                        WriteOrder(o);
                }
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--8. Alle klanten met een order met DeliveryDate binnen de 10 dagen, sorteer op naam--");
                customers = context.Customers.Include(c => c.Orders).Where(c => c.Orders.Any(o => o.DeliveryDate.Value < DateTime.Now.AddDays(10))).OrderBy(c => c.Name);
                WriteCustomers(customers);
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--9. Alle klanten die een product met id 1  hebben besteld--");
                product = context.Products.FirstOrDefault(p => p.ProductId == 1);
                customers = context.Customers.Include(c => c.Orders).ThenInclude(o => o.OrderLines).ThenInclude(ol => ol.Product).ToList().Where(c => c.Orders.Any(o => o.HasOrdered(product)));
                WriteCustomers(customers);
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--10. Alle klanten met orders, met vermelding van aantal orders. Maak gebruik van een anoniem type--");
                var customers2 = context.Customers.Include(c => c.Orders).Where(c => c.Orders.Count > 0);

                foreach (var c in customers2)
                {
                    var customerPrint = new
                    {
                        c.Name,
                        AmountOfOrders = c.Orders.Count
                    };
                    Console.WriteLine(customerPrint.Name + " " + customerPrint.AmountOfOrders);
                }
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--11. Pas de naam aan van klant student5, in je eigen naam en voornaam--");
                customer = context.Customers.SingleOrDefault(c => c.CustomerName == "student5");
                customer.Name = "MijnNaam";
                customer.FirstName = "MijnVoornaam";
                context.SaveChanges();
                customers = context.Customers;
                WriteCustomers(customers);
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--12A. Verwijder de eerste klant (in alfabetische volgorde van customername) zonder orders--");
                customer = context.Customers.Include(c => c.Orders).OrderBy(c => c.CustomerName).FirstOrDefault(c => c.Orders.Count == 0);
                context.Customers.Remove(customer);
                context.SaveChanges();
                customers = context.Customers.OrderBy(c => c.CustomerName);
                WriteCustomers(context.Customers);
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--13. Maak een nieuw product training, prijs 80, aan die behoort tot de categorie soccer en watersports en print na toevoeging aan de database het productid af--");
                Product training = new Product("training", 80, "Adidas Performance CONDIVO 14");
                Category soccer = context.Categories.SingleOrDefault(c => c.Name == "Soccer");
                Category watersports = context.Categories.SingleOrDefault(c => c.Name == "WaterSports");
                soccer.AddProduct(training);
                watersports.AddProduct(training);
                context.SaveChanges();
                Console.WriteLine(soccer.FindProduct("training")?.ProductId);

                Console.WriteLine("\n--14. Product training behoort niet langer tot de category soccer. Ga na of CategoryProduct ook verwijderd wordt-");
                if (training != null)
                    soccer.RemoveProduct(training);
                context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Name == "Soccer").Products.ToList().ForEach(cp => Console.WriteLine(cp.ProductId));
                context.SaveChanges();
                Console.ReadLine();
            }

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Console.WriteLine("\n--15. Probeer de stad Gent te verwijderen. Waarom lukt dit niet?--");
                City city = context.Cities.FirstOrDefault(c => c.Name == "Gent");
                try
                {
                    context.Cities.Remove(city);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Verwijderen Gent faalt. Reason : {ex.InnerException.Message}");
                }
            }
            Console.ReadKey();
        }

        private static void WriteOrder(Order o)
        {
            if (o != null)
                Console.WriteLine($"{o.DeliveryDate} {o.OrderDate} {o.Total}");
        }


        private static void WriteCustomers(IEnumerable<Customer> customers)
        {
            if (customers != null)
                customers.ToList().ForEach(c => Console.WriteLine($"{c.Name} {c.FirstName}"));
        }

        private static void WriteProducts(IEnumerable<Product> products)
        {
            if (products != null)
                products.ToList().ForEach(p => Console.WriteLine($"{p.ProductId} {p.Name} {p.Price:0.00}"));
        }
    }

}
