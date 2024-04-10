using Microsoft.EntityFrameworkCore;
using Pos.Web.Data.Entities;

namespace Pos.Web.Data.Seeders
{
    internal class SalesSeeder
    {
        private readonly DataContext _context;

        public SalesSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Sales> salesA = new List<Sales>
            {
                 new Sales
                 {
                     DateSales = DateTime.Now,
                     DiscountsSales = 10.0m,
                     TotalSales = 100.0m,
                     PaymentMethod = "Efectivo",
                     SalesType = "Venta al contado",
                     CustomerId = 1  // Reemplaza 1 con el ID del cliente correspondiente
                 },
                 new Sales
                 {
                     DateSales = DateTime.Now.AddDays(-1),
                     DiscountsSales = 5.0m,
                     TotalSales = 50.0m,
                     PaymentMethod = "Tarjeta de crédito",
                     SalesType = "Venta a crédito",
                     CustomerId = 2  // Reemplaza 2 con el ID del cliente correspondiente
                 },
                 new Sales
                 {
                    DateSales = DateTime.Now.AddDays(-2),
                    DiscountsSales = 15.0m,
                    TotalSales = 200.0m,
                    PaymentMethod = "Tarjeta de débito",
                    SalesType = "Venta al contado",
                    CustomerId = 3  // Reemplaza 3 con el ID del cliente correspondiente
                 },
                 new Sales
                 {
                    DateSales = DateTime.Now.AddDays(-3),
                    DiscountsSales = 8.0m,
                    TotalSales = 150.0m,
                    PaymentMethod = "Transferencia bancaria",
                    SalesType = "Venta a crédito",
                    CustomerId = 4  // Reemplaza 4 con el ID del cliente correspondiente
                 },
                 new Sales
                 {
                    DateSales = DateTime.Now.AddDays(-4),
                    DiscountsSales = 20.0m,
                    TotalSales = 300.0m,
                    PaymentMethod = "Efectivo",
                    SalesType = "Venta al contado",
                    CustomerId = 5  // Reemplaza 5 con el ID del cliente correspondiente
                 },
            };

            foreach (Sales sales in salesA)
            {
                bool exists = await _context.Sales.AnyAsync(s => s.CustomerId == sales.CustomerId);

                if (!exists)
                {
                    await _context.Sales.AddAsync(sales);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}