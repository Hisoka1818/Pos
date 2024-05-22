using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Data.Entities;
using Pos.Web.Services;


namespace Pos.Web.Data.Seeders
{
    public class UserRoleSeeder
    {
        private readonly IUsersService _usersService;
        private readonly DataContext _context;

        public UserRoleSeeder(IUsersService usersService, DataContext context)
        {
            _usersService = usersService;
            _context = context;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsers();
        }

        private async Task AdministradorRoleAsync()
        {
            PrivatePosRole? tmp = await _context.PrivatePosRoles.Where(ir => ir.Name == Constants.SUPER_ADMIN_ROLE_NAME).FirstOrDefaultAsync();

            if (tmp == null)
            {
                PrivatePosRole role = new PrivatePosRole { Name = Constants.SUPER_ADMIN_ROLE_NAME };
                _context.PrivatePosRoles.Add(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task GestorDeUsuariosRoleAsync()
        {
            PrivatePosRole? tmp = await _context.PrivatePosRoles.Where(pbr => pbr.Name == "Gestor de usuarios").FirstOrDefaultAsync();

            if (tmp == null)
            {
                PrivatePosRole role = new PrivatePosRole { Name = "Gestor de usuarios" };

                _context.PrivatePosRoles.Add(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Usuarios").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    _context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task EmpleadoRoleAsync()
        {
            PrivatePosRole? tmp = await _context.PrivatePosRoles.Where(pbr => pbr.Name == "Empleado").FirstOrDefaultAsync();

            if (tmp == null)
            {
                PrivatePosRole role = new PrivatePosRole { Name = "Empleado" };

                _context.PrivatePosRoles.Add(role);
                //Mirar Important
                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Empleados").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    _context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task SupervisorRoleAsync()
        {
            PrivatePosRole? tmp = await _context.PrivatePosRoles.Where(pbr => pbr.Name == "Supervisor").FirstOrDefaultAsync();

            if (tmp == null)
            {
                PrivatePosRole role = new PrivatePosRole { Name = "Supervisor" };

                _context.PrivatePosRoles.Add(role);
                //Mirar Important
                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Supervisores").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    _context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
                }
            }

            await _context.SaveChangesAsync();
        }


        private async Task CheckUsers()
        {
            // Administrador
            User? user = await _usersService.GetUserAsync("Jhon@yopmail.com");

            PrivatePosRole adminRole = _context.PrivatePosRoles.Where(r => r.Name == "Administrador").First();

            if (user is null)
            {
                user = new User
                {
                    Email = "Jhon@yopmail.com",
                    FirstName = "Jhon Fredy",
                    LastName = "Gomez Escobar",
                    PhoneNumber = "3000000000",
                    UserName = "Jhon@yopmail.com",
                    Document = "1111",
                    PrivatePosRole = adminRole,
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // Empleado
            user = await _usersService.GetUserAsync("Daniela@yopmail.com");


            PrivatePosRole empleadoRole = await _context.PrivatePosRoles.Where(pbr => pbr.Name == "Empleado").FirstAsync();

            if (user == null)
            {
                user = new User
                {
                    Email = "Daniela@yopmail.com",
                    FirstName = "Daniela",
                    LastName = "Londres",
                    PhoneNumber = "30000000",
                    UserName = "Daniela@yopmail.com",
                    Document = "2222",
                    PrivatePosRole = empleadoRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // Supervisor
            user = await _usersService.GetUserAsync("Albert@yopmail.com");


            PrivatePosRole supervisorRole = await _context.PrivatePosRoles.Where(pbr => pbr.Name == "Supervisor").FirstAsync();

            if (user == null)
            {
                user = new User
                {
                    Email = "Albert@yopmail.com",
                    FirstName = "Albert",
                    LastName = "Paris",
                    PhoneNumber = "60000000",
                    UserName = "Albert@yopmail.com",
                    Document = "3333",
                    PrivatePosRole = supervisorRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // Gestor de usuarios
            user = await _usersService.GetUserAsync("David@yopmail.com");

            PrivatePosRole gestorDeUsuarios = await _context.PrivatePosRoles.Where(pbr => pbr.Name == "Gestor de usuarios").FirstAsync();

            if (user == null)
            {
                user = new User
                {
                    Email = "David@yopmail.com",
                    FirstName = "David",
                    LastName = "Doe",
                    PhoneNumber = "40000000",
                    UserName = "David@yopmail.com",
                    Document = "4444",
                    PrivatePosRole = gestorDeUsuarios
                };

                var result = await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }
        }

        private async Task CheckRolesAsync()
        {
            await AdministradorRoleAsync();
            await GestorDeUsuariosRoleAsync();
            await EmpleadoRoleAsync();
            await SupervisorRoleAsync(); 
        }
    }

}

