using Pos.Web.Data.Entities;

namespace Pos.Web.Data.Seeders
{
    public class PermissionSeeder
    {
        private readonly DataContext _context;

        public PermissionSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = new List<Permission>();
            permissions.AddRange(Roles());
            permissions.AddRange(Users());
            permissions.AddRange(Employee()); 
            permissions.AddRange(Supervisor());

            foreach (Permission permission in permissions)
            {
                Permission? tmpPermission = _context.Permissions.Where(p => p.Name == permission.Name && p.Module == permission.Module)
                                                                .FirstOrDefault();
                if (tmpPermission is null)
                {
                    _context.Permissions.Add(permission);
                }
            }

            await _context.SaveChangesAsync();
        }


        private List<Permission> Roles()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showRoles", Description = "Ver Roles", Module = "Roles" },
                new Permission { Name = "createRoles", Description = "Crear Roles", Module = "Roles" },
                new Permission { Name = "updateRoles", Description = "Editar Roles", Module = "Roles" },
                new Permission { Name = "deleteRoles", Description = "Eliminar Roles", Module = "Roles" },
            };

            return list;
        }

        private List<Permission> Employee()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showEmployees", Description = "Ver Empleados", Module = "Empleados" },
                new Permission { Name = "createEmployees", Description = "Crear Empleados", Module = "Empleados" },
                new Permission { Name = "updateEmployees", Description = "Editar Empleados", Module = "Empleados" },
                new Permission { Name = "deleteEmployees", Description = "Eliminar Empleados", Module = "Empleados" },
            };

            return list;
        }
        
        private List<Permission> Supervisor()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showSupervisores", Description = "Ver Supervisores", Module = "Supervisores" },
                new Permission { Name = "createSupervisores", Description = "Crear Supervisores", Module = "Supervisores" },
                new Permission { Name = "updateSupervisores", Description = "Editar Supervisores", Module = "Supervisores" },
                new Permission { Name = "deleteSupervisores", Description = "Eliminar Supervisores", Module = "Supervisores" },
            };

            return list;
        }

        private List<Permission> Users()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showUsers", Description = "Ver Usuarios", Module = "Usuarios" },
                new Permission { Name = "createUsers", Description = "Crear Usuarios", Module = "Usuarios" },
                new Permission { Name = "updateUsers", Description = "Editar Usuarios", Module = "Usuarios" },
                new Permission { Name = "deleteUsers", Description = "Eliminar Usuarios", Module = "Usuarios" },
            };

            return list;
        }
    }
}
