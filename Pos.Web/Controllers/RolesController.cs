using Microsoft.AspNetCore.Mvc;
using Pos.Web.Core;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core.Attributes;
using Pos.Web.Core.Pagination;
using Pos.Web.Data.Entities;
using Pos.Web.Services;
using PrivatePos.Web.DTOs;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Pos.Web.Controllers
{
    public class RolesController : Controller
    {
        private IRolesService _rolesService;
        private readonly INotyfService _noty;

        public RolesController(IRolesService rolesService, INotyfService noty)
        {
            _rolesService = rolesService;
            _noty = noty;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showRoles" , module: "Roles")]//Aqui pongo los que pueden entrar a ver esta entidad en este momento solo el supervisor pude hacerlo 
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {
            PaginationRequest paginationRequest = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 5, //15
                Page = Page ?? 1,
                Filter = Filter,
            };

            Response<PaginationResponse<PrivatePosRole>> permissionsResponse = await _rolesService.GetListAsync(paginationRequest);

            return View(permissionsResponse.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "CreateRoles" , module: "Roles")]
        public async Task<IActionResult> Create()
        {
            Response<IEnumerable<Permission>> permissionsResponse = await _rolesService.GetPermissionsAssync();

            if(!permissionsResponse.IsSuccess)
            {
                _noty.Error(permissionsResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            PrivatePosRoleDTO dTO = new PrivatePosRoleDTO
            {
                permissions = permissionsResponse.Result.Select(p=> new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList()
            };
            return View(dTO);
        }

        [HttpGet]
        [CustomAuthorize(permission: "CreateRoles" , module: "Roles")]
        public async Task<IActionResult> Create(PrivatePosRoleDTO dTO)
        {
            Response <IEnumerable<Permission>> permissionsResponse =await _rolesService.GetPermissionsAsync();
            if(!ModelState.IsValid)
            {
                _noty.Error("Debe ajustar los errores de validacion.");

                
                dTO.permissions = permissionsResponse.Result.Select(p=> new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList();

                return View(dTO);
            }

            Response<PrivatePosRole> CreateResponse = await _rolesService.CreateAsync(dTO);
            
            if(!CreateResponse.IsSuccess)
            {
                _noty.Success(CreateResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _noty.Error(CreateResponse.Message);
            dTO.permissions = permissionsResponse.Result.Select(p=> new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList();

                return View(dTO);
        }

        [HttpGet]
        [CustomAuthorize(permission: "updateRoles" , module: "Roles")]
        public async Task<IActionResult> Edit(int id)
        {
            Response<PrivatePosRoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _noty.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateRoles" , module: "Roles")]
        public async Task<IActionResult> Edit(PrivatePosRoleDTO dto)
        {
            Response<IEnumerable<PermissionForDTO>> res = await _rolesService.GetPermissionsByRoleAsync(dto.Id);

            if(!ModelState.IsValid)
            {
                _noty.Error("Debe ajustar los errores de validacion.");

                
                dto.permissions = res.Result.ToList();

                return View(dto);
            }

            Response<PrivatePosRole> response = await _rolesService.EditAsync(dto);
            
            if(response.IsSuccess)
            {
                _noty.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _noty.Error(response.Message);
                dto.permissions = res.Result.ToList();
                return View(dto);
        }
    }
}