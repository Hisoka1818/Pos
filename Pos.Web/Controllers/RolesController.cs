using Microsoft.AspNetCore.Mvc;
using Pos.Web.Core;
using Pos.Web.Core.Attributes;
using Pos.Web.Core.Pagination;
using Pos.Web.Data.Entities;
using Pos.Web.Services;

namespace Pos.Web.Controllers
{
    public class RolesController : Controller
    {
        private IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService=rolesService;
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

            Response<PaginationResponse<PrivatePosRole>> response = await _rolesService.GetListAsync(paginationRequest);

            return View(response.Result);
        }

    }
}