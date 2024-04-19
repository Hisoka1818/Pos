using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Core.Pagination;
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

            Response<PaginationResponse<Sales>> response = await _salesService.GetListAsync(paginationRequest);

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

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            try
            {
                Sales? sales = await _context.Sales.Include(b => b.Customer).FirstOrDefaultAsync(b => b.Id == id);

                if (sales is null)
                {
                    return RedirectToAction(nameof(Index));
                }

                SalesDTO salesDTO = new SalesDTO
                {
                    Id = id,
                    CustomerId = sales.CustomerId,
                    DateSales = sales.DateSales,
                    SalesType = sales.SalesType,
                    PaymentMethod = sales.PaymentMethod,
                    DiscountsSales = sales.DiscountsSales,
                    TotalSales = sales.TotalSales,
                    Customer = await _context.Customer.Select(a => new SelectListItem
                    {
                        Text = a.FirstName,
                        Value = a.Id.ToString(),
                    }).ToArrayAsync(),

                };

                return View(salesDTO);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(SalesDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Customer = await _context.Customer.Select(a => new SelectListItem
                    {
                        Text = a.FirstName,
                        Value = a.Id.ToString(),
                    }).ToArrayAsync();
                    return View(model);
                }
                Sales sales = await _context.Sales.FirstOrDefaultAsync(a => a.Id == model.Id);

                if (sales is null)
                {
                    return NotFound();
                }

                Response<Sales> response = await _salesService.UpdateAsync(model);

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

        [HttpPost]
        public async Task<IActionResult> Delete(CustomerDTO model, [FromRoute] int id)
        {
            try
            {
                Sales sales = await _context.Sales.FirstOrDefaultAsync(a => a.Id == id);

                if (sales is null)
                {
                    return RedirectToAction(nameof(Index));
                }

                Response<Sales> response = await _salesService.DeleteAsync(id);
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
