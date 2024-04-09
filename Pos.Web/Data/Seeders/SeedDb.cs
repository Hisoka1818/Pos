using System.Reflection.PortableExecutable;

namespace Pos.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await new CustomerSeeder(_context).SeedAsync();
            await new SalesSeeder(_context).SeedAsync();
        }
    }
}
