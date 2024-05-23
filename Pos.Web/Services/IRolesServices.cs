using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core.Pagination;
using Pos.Web.Core;
using Pos.Web.Data.Entities;
using Pos.Web.Data;
using Pos.Web.DTOs;
using Pos.Web.Helpers;

namespace PrivatePos.Web.Services
{
    public interface IRolesService
    {
        public Task<Response<PrivatePosRole>> CreateAsync(PrivatePosRoleDTO dto);

        public Task<Response<object>> DeleteAsync(int id);

        public Task<Response<PrivatePosRole>> EditAsync(PrivatePosRoleDTO dto);

        public Task<Response<PaginationResponse<PrivatePosRole>>> GetListAsync(PaginationRequest request);

        public Task<Response<PrivatePosRoleDTO>> GetOneAsync(int id);

        public Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();

        public Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id);
    }

    public class RolesService : IRolesService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public RolesService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<PrivatePosRole>> CreateAsync(PrivatePosRoleDTO dto)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Creación de Rol
                    PrivatePosRole model = _converterHelper.ToRole(dto);
                    EntityEntry<PrivatePosRole> modelStored = await _context.PrivatePosRoles.AddAsync(model);

                    await _context.SaveChangesAsync();

                    // Inserción de permisos
                    int roleId = modelStored.Entity.Id;

                    List<int> permissionIds = new List<int>();

                    if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                    {
                        permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                    }

                    foreach (int permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permissionId
                        };

                        _context.RolePermissions.Add(rolePermission);
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return ResponseHelper<PrivatePosRole>.MakeResponseSuccess("Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ResponseHelper<PrivatePosRole>.MakeResponseFail(ex);
                }
            }
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            try
            {
                Response<PrivatePosRole> roleResponse = await GetOneModelAsync(id);

                if (!roleResponse.IsSuccess)
                {
                    return ResponseHelper<object>.MakeResponseFail(roleResponse.Message);
                }

                if (roleResponse.Result.Name == Constants.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<object>.MakeResponseFail($"El rol {Constants.SUPER_ADMIN_ROLE_NAME} no puede ser eliminado");
                }

                if (roleResponse.Result.Users.Count() > 0)
                {
                    return ResponseHelper<object>.MakeResponseFail($"El rol no puede ser eliminado debido a que tiene usuarios relacionados");
                }

                _context.PrivatePosRoles.Remove(roleResponse.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<object>.MakeResponseSuccess("Rol eliminado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<object>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PrivatePosRole>> EditAsync(PrivatePosRoleDTO dto)
        {
            try
            {
                if (dto.Name == Constants.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<PrivatePosRole>.MakeResponseFail($"El Rol '{Constants.SUPER_ADMIN_ROLE_NAME}' no puede ser editado");
                }

                List<int> permissionIds = new List<int>();

                if (!string.IsNullOrEmpty(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                // Eliminación de antiguos permisos
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rs => rs.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // Inserción de nuevos permisos
                foreach (int permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = dto.Id,
                        PermissionId = permissionId
                    };

                    _context.RolePermissions.Add(rolePermission);
                }

                // Actualización de rol
                PrivatePosRole model = _converterHelper.ToRole(dto);
                _context.PrivatePosRoles.Update(model);

                await _context.SaveChangesAsync();

                return ResponseHelper<PrivatePosRole>.MakeResponseSuccess("Rol editado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PrivatePosRole>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<PrivatePosRole>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<PrivatePosRole> queryable = _context.PrivatePosRoles.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    queryable = queryable.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<PrivatePosRole> list = await PagedList<PrivatePosRole>.ToPagedListAsync(queryable, request);

                PaginationResponse<PrivatePosRole> result = new PaginationResponse<PrivatePosRole>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,
                };

                return ResponseHelper<PaginationResponse<PrivatePosRole>>.MakeResponseSuccess(result, "Roles obtenidas con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<PrivatePosRole>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PrivatePosRoleDTO>> GetOneAsync(int id)
        {
            try
            {
                PrivatePosRole? privatePosRole = await _context.PrivatePosRoles.FirstOrDefaultAsync(r => r.Id == id);

                if (privatePosRole is null)
                {
                    return ResponseHelper<PrivatePosRoleDTO>.MakeResponseFail($"El Rol con id '{id}' no existe.");
                }

                return ResponseHelper<PrivatePosRoleDTO>.MakeResponseSuccess(await _converterHelper.ToRoleDTOAsync(privatePosRole));
            }
            catch (Exception ex)
            {
                return ResponseHelper<PrivatePosRoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            try
            {
                IEnumerable<Permission> permissions = await _context.Permissions.ToListAsync();

                return ResponseHelper<IEnumerable<Permission>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<Permission>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id)
        {
            try
            {
                Response<PrivatePosRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(response.Message);
                }

                List<PermissionForDTO> permissions = response.Result.Permissions;

                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(ex);
            }
        }

        private async Task<Response<PrivatePosRole>> GetOneModelAsync(int id)
        {
            try
            {
                PrivatePosRole? role = await _context.PrivatePosRoles.Include(r => r.Users)
                                                                       .FirstOrDefaultAsync(r => r.Id == id);

                if (role is null)
                {
                    return ResponseHelper<PrivatePosRole>.MakeResponseFail($"El Rol con id '{id}' no existe");
                }

                return ResponseHelper<PrivatePosRole>.MakeResponseSuccess(role);

            }
            catch (Exception ex)
            {
                return ResponseHelper<PrivatePosRole>.MakeResponseFail(ex);
            }
        }
    }
}