using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Pos.Web.Core.Attributes;
using Pos.Web.Core.Pagination;
using Pos.Web.Core;
using Pos.Web.Data.Entities;
using Pos.Web.Services;
using Pos.Web.Helpers;
using Pos.Web.DTOs;

namespace Pos.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _noty;
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService, ICombosHelper combosHelper, INotyfService noty)
        {
            _usersService = usersService;
            _combosHelper = combosHelper;
            _noty = noty;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showUsers", module: "Usuarios")]
        public async Task<IActionResult> Index([FromQuery] int? Page,
                                               [FromQuery] int? RecordsPerPage,
                                               [FromQuery] string? Filter)
        {
            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter
            };

            return View(await _usersService.GetUsersPaginatedAsync(request));
        }

        [HttpGet]
        [CustomAuthorize(permission: "createUsers", module: "Usuarios")]
        public async Task<IActionResult> Create()
        {
            return View(new UserDTO
            {
                IsNew = true,
                PrivateBlogRoles = await _combosHelper.GetComboPrivateBlogRolesAsync()
            });
        }

        [HttpPost]
        [CustomAuthorize(permission: "createUsers", module: "Usuarios")]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _noty.Error("Debe ajustar los errores de validación.");
                dto.PrivateBlogRoles = await _combosHelper.GetComboPrivateBlogRolesAsync();
                return View(dto);
            }

            Response<User> response = await _usersService.CreateAsync(dto);

            if (response.IsSuccess)
            {
                _noty.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _noty.Error(response.Message);
            dto.PrivateBlogRoles = await _combosHelper.GetComboPrivateBlogRolesAsync();
            return View(dto);
        }

    }
}
