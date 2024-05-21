using System.Reflection.PortableExecutable;
using AspNetCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Pos.Web.Core;
using Pos.Web.Core.Pagination;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.Helpers;
using PrivatePos.Web.DTOs;

namespace Pos.Web.Services
{
    public interface IRolesService
    {
        
        public Task<Response<PrivatePosRole>> CreateAsync(PrivatePosRoleDTO dTO);
        public Task<Response<PrivatePosRole>> EditAsync(PrivatePosRoleDTO dTO);
        public Task<Response<PaginationResponse<PrivatePosRole>>> GetListAsync(PaginationRequest request);
        public Task<Response<PrivatePosRoleDTO>> GetOneAsync(int id);
        public Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();
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

        public async Task<Response<PrivatePosRole>> CreateAsync(PrivatePosRoleDTO dTO)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync)
                {
                    try
                    {
                        //Creacion de Rol
                        PrivatePosRole model = _converterHelper.ToRole(dto);
                        EntityEntry<PrivatePosRole> modelStored = await _context.PrivatePosRoles.AddAsync(model);

                        await _context.SaveChangesAsync();

                        //Insercion de permisos
                        int roleId = modelStored.Entity.Id;

                        List<int> permissionIds = new List<int>();

                        if(!string.IsNullOrWhiteSpace(dTO.PermissionIds))
                        {
                            permissionIds = JsonConvert.DeserializeObject<List<int>>(dTO.PermissionIds);
                        }

                        foreach(int permissionId in permissionIds)
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

                        return ResponseHelper<PrivatePosRole>.MakeResponseSuccess("Rol creado con exito");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return ResponseHelper<PrivatePosRole>.MakeResponseFail(ex);
                    }
                }
            
        }

        public async Task<Response<PrivatePosRole>> EditAsync(PrivatePosRoleDTO dto)
        {
            try
            {
                if(dto.Name == Constants.SUPER_ADMIN_ROLE_NAME)
                {
                    return ResponseHelper<PrivatePosRole>.MakeResponseFail($"El rol '{Constants.SUPER_ADMIN_ROLE_NAME}'no puede ser editado");
                }

                //Eliminar permisos antiguos
                List<RolePermission>rolePermissions = await _context.RolePermissions.Where(rp => rp.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(rolePermissions);

                //insercion permisos
                List<int> permissionIds = new List<int>();

                        if(!string.IsNullOrWhiteSpace(dto.PermissionIds))
                        {
                            permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                        }

                        foreach(int permissionId in permissionIds)
                        {
                            RolePermission rolePermission = new RolePermission
                            {
                                RoleId = dto.Id,
                                PermissionId = permissionId
                            };

                            _context.RolePermissions.Add(rolePermission);
                        }

                        //Actualizacion de Rol
                        PrivatePosRole model=_converterHelper.ToRole(dto);
                        _context.PrivatePosRoles.Update(model);

                        await _context.SaveChangesAsync();

                        return ResponseHelper<PrivatePosRole>.MakeResponseSuccess("Rol editado correctamente");
            }
            catch(Exception ex)
            {
                return ResponseHelper<PrivatePosRole>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<PrivatePosRole>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<PrivatePosRole>queryable=_context.PrivatePosRoles.AsQueryable();

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

                return ResponseHelper<PaginationResponse<PrivatePosRole>>.MakeResponseSuccess(result, "Roles obtenidos con exito");
            }
            catch(Exception ex)
            {
                return ResponseHelper<PaginationResponse<PrivatePosRole>>.MakeResponseFail(ex);
            }
        }

        public Task<Response<PrivatePosRoleDTO>> GetOneAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Response<IEnumerable<Permission>>> GetPersmissionsAsync()
        {
            try
            {
                IEnumerable<Permission> permissions = await _context.Permissions.ToListAsync();

                return ResponseHelper<IEnumerable<Permission>>.MakeResponseSucces(Permissions);
            }
            catch(Exception ex)
            {
                return ResponseHelper<IEnumerable<Permission>>.MakeResponseFail(ex);
            }

        }
    }
}