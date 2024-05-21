

using Microsoft.EntityFrameworkCore;
using Pos.Web.Data.Entities;

namespace Pos.Web.Data.Seeders
{
    internal class CustomerSeeder
    {
        private readonly DataContext _context;

        public CustomerSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Customer> customers = new List<Customer>
            {
                 new Customer 
                 { 
                     FirstName = "Juan", 
                     LastName = "Pérez", 
                     PhoneNumber = "123456789", 
                     EmailAddress = "juan@example.com", 
                     CustomerType = "Nuevo" 
                 },
                 new Customer 
                 {
                     FirstName = "María",
                     LastName = "Gómez",
                     PhoneNumber = "987654321",
                     EmailAddress = "maria@example.com",
                     CustomerType = "Regular"

                 },
                 new Customer
                 {
                    FirstName = "Ana",
                    LastName = "García",
                    PhoneNumber = "555-1234",
                    EmailAddress = "ana@example.com",
                    CustomerType = "Frecuente"
                 },
                 new Customer
                 {
                    FirstName = "Pedro",
                    LastName = "López",
                    PhoneNumber = "555-5678",
                    EmailAddress = "pedro@example.com",
                    CustomerType = "Nuevo"
                 },
                 new Customer
                 {
                    FirstName = "Marcela",
                    LastName = "Hernández",
                    PhoneNumber = "555-9012",
                    EmailAddress = "marcela@example.com",
                    CustomerType = "Regular"
                 },
                 new Customer
                 {
                   FirstName = "Juan", 
                    LastName = "Pérez",
                    PhoneNumber = "555-1234",
                    EmailAddress = "juan@example.com",
                    CustomerType = "Premium"
                 },
                 new Customer
                 {
                    FirstName = "Ana",
                    LastName = "García",
                    PhoneNumber = "555-5678",
                    EmailAddress = "ana@example.com",
                    CustomerType = "Regular"
                 },
                 new Customer
                 {
                    FirstName = "Pedro",
                    LastName = "López",
                    PhoneNumber = "555-2468",
                    EmailAddress = "pedro@example.com",
                    CustomerType = "Premium"
                 },
                 new Customer
                 {
                    FirstName = "Luisa",
                    LastName = "Martínez",
                    PhoneNumber = "555-9876",
                    EmailAddress = "luisa@example.com",
                    CustomerType = "Regular"
                 },
                 new Customer
                 {
                    FirstName = "Miguel",
                    LastName = "Rodríguez",
                    PhoneNumber = "555-1357",
                    EmailAddress = "miguel@example.com",
                    CustomerType = "Premium"
                 },
                 new Customer
                 {
                    FirstName = "Sofía",
                    LastName = "Gómez",
                    PhoneNumber = "555-3690",
                    EmailAddress = "sofia@example.com",
                    CustomerType = "Regular"
                 },
                 new Customer
                 {
                    FirstName = "Javier",
                    LastName = "Sánchez",
                    PhoneNumber = "555-2468",
                    EmailAddress = "javier@example.com",
                    CustomerType = "Premium"
                 },
                 new Customer
                 {
                    FirstName = "María",
                    LastName = "Fernández",
                    PhoneNumber = "555-7531",
                    EmailAddress = "maria@example.com",
                    CustomerType = "Regular"
                 },
                 new Customer
                 {
                    FirstName = "Carlos",
                    LastName = "Ramírez",
                    PhoneNumber = "555-8024",
                    EmailAddress = "carlos@example.com",
                    CustomerType = "Premium"
                 },
                 new Customer
                 {
                    FirstName = "Laura",
                    LastName = "Díaz",
                    PhoneNumber = "555-2468",
                    EmailAddress = "laura@example.com",
                    CustomerType = "Regular"
                 }
            };

            foreach (Customer customer in customers)
            {
                bool exists = await _context.Customer.AnyAsync(s => s.FirstName == customer.FirstName);

                if (!exists)
                {
                    await _context.Customer.AddAsync(customer);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}