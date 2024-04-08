using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTO;
using Pos.Web.Services;


namespace Pos.Web.Controllers
{
  


    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly IProductsService _productsService;
        private readonly INotyfService _notify;

        public ProductsController(IProductsService productsService, INotyfService notify, DataContext context)
        {
            _productsService = productsService;
            _notify = notify;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Response<List<Products>> response = await _productsService.GetListAsync();
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProductsDTO productsDTO = new ProductsDTO
            {
                Categories = await _context.Categories.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                }).ToArrayAsync(),
            };
            return View(productsDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductsDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notify.Error("Debe ajustar los errores de validación.");
                    return View(model);
                }

                Response<Products> response = await _productsService.CreateAsync(model);

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