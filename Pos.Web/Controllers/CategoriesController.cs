using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Web.Core;
using Pos.Web.Core.Pagination;
using Pos.Web.Data.Entities;
using Pos.Web.Services;
using static Pos.Web.Services.ICustomerService;

namespace Pos.Web.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesServices;
        private readonly INotyfService _notify;

        public CategoriesController(ICategoriesService categoriesServices, INotyfService notify)
        {
            _categoriesServices = categoriesServices;
            _notify = notify;
        }

        [HttpGet]
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

            Response<PaginationResponse<Categories>> response = await _categoriesServices.GetListAsync(paginationRequest);
            return View(response.Result);
        }
        [HttpGet] 
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Categories model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    _notify.Error("Debe ajustar los errores de validación.");
                    return View(model);
                }

                Response<Categories> response = await _categoriesServices.CreateAsync(model);

                if (response.IsSuccess)
                {
                    _notify.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notify.Error(response.Message);
                return View(model);

            }
            catch (Exception ex)
            {
                _notify.Error(ex.Message);
                return View(model);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Categories> response = await _categoriesServices.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notify.Error(response.Errors.First());
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<IActionResult> Update(Categories model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notify.Error("Debe ajustar los errores de validación");
                    return View(model);
                }

                Response<Categories> response = await _categoriesServices.EditAsync(model);

                if (response.IsSuccess)
                {
                    _notify.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notify.Error(response.Errors.First());
                return View(model);

            }
            catch (Exception ex)
            {
                _notify.Error(ex.Message);
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Categories> response = await _categoriesServices.DeleteAsync(id);

            if (response.IsSuccess)
            {
                _notify.Success(response.Message);
                return RedirectToAction(nameof(Index));

            }

            _notify.Error(response.Errors.First());
            return RedirectToAction(nameof(Index));
        }

    }
}
