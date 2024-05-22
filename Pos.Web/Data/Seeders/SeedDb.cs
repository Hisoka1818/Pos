using Pos.Web.Services;
using System.Reflection.PortableExecutable;

namespace Pos.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public SeedDb(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await new CustomerSeeder(_context).SeedAsync();
            await new SalesSeeder(_context).SeedAsync();
            await new PermissionSeeder(_context).SeedAsync();
            await new UserRoleSeeder(_usersService, _context).SeedAsync();
            await new CategoriesSeeder(_context).SeedAsync();
        }
    }
}
