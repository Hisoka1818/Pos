using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTOs;
using Pos.Web.Services;


namespace Pos.Web.Controllers
{
    public class SalesController : Controller
    {
        private readonly DataContext _context;
        private readonly ISalesService _salesService;
        private readonly INotyfService _notify;

        public SalesController(ISalesService salesService, INotyfService notify, DataContext context)
        {
            _salesService = salesService;
            _notify = notify;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Response<List<Sales>> response = await _salesService.GetListAsync();
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            SalesDTO salesDTO = new SalesDTO
            {
                Customer = await _context.Customer.Select(a => new SelectListItem
                {
                    Text = a.FirstName,
                    Value = a.Id.ToString(),
                }).ToArrayAsync(),
            };
            return View(salesDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SalesDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notify.Error("Debe ajustar los errores de validación.");
                    return View(model);
                }
                
                Response<Sales> response = await _salesService.CreateAsync(model);

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
    }

}
