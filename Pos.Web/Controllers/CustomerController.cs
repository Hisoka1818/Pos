using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Web.Core;
using Pos.Web.Core.Attributes;
using Pos.Web.Core.Pagination;
using Pos.Web.Data.Entities;
using Pos.Web.Services;


namespace Pos.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly INotyfService _notify;

        public CustomerController(ICustomerService customerService, INotyfService notify)
        {
            _customerService = customerService;
            _notify = notify;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showSupervisores" , module: "Supervisores")]//Aqui pongo los que pueden entrar a ver esta entidad en este momento solo el supervisor pude hacerlo 
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

            Response<PaginationResponse<Customer>> response = await _customerService.GetListAsync(paginationRequest);

            return View(response.Result);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    _notify.Error("Debe ajustar los errores de validación.");
                    return View(model);
                }

                Response<Customer> response = await _customerService.CreateAsync(model);

                if (response.IsSuccess)
                {
                    _notify.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notify.Error(response.Message);
                return View(model);

            } catch (Exception ex)
            {
                _notify.Error(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Customer> response = await _customerService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notify.Error(response.Errors.First());
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Update(Customer model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notify.Error("Debe ajustar los errores de validación.");
                    return View(model);
                }

                Response<Customer> response = await _customerService.EditAsync(model);

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
            Response<Customer> response = await _customerService.DeleteAsync(id);

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
