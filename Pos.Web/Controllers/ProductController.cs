using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTOs;
using Pos.Web.Services;
using Pos.Web.Core.Pagination;
using static Pos.Web.Services.ICategoriesService;



namespace Pos.Web.Controllers
{

    public class ProductController : Controller
    {
        private readonly DataContext _context;
        private readonly IProductService _productService;
        private readonly INotyfService _notify;

        public ProductController(IProductService productService, INotyfService notify, DataContext context)
        {
            _productService = productService;
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

            Response<PaginationResponse<Categories>> response = await _productServices.GetListAsync(paginationRequest);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProductDTOs productDTOs = new ProductDTOs
            {
                Categories = await _context.Categories.Select(a => new SelectListItem
                {
                    Text = a.categoryName,
                    Value = a.Id.ToString(),
                }).ToArrayAsync(),
            };
            return View(productDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTOs model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notify.Error("Debe ajustar los errores de validación.");
                    return View(model);
                }

                Response<Product> response = await _productService.CreateAsync(model);

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
                Product? product = await _context.Products.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == id);

                if (product is null)
                {

                    return RedirectToAction(nameof(Index));
                }

                ProductDTOs productDTOs = new ProductDTOs
                {
                    Id = id,
                    CategoriesId = product.CategoriesId,
                    Name = product.Name,
                    price = product.price,
                    reference = product.reference,
                    area = product.area,
                    Categories = await _context.Categories.Select(a => new SelectListItem

                    {
                        Text = a.categoryName,
                        Value = a.Id.ToString(),
                    }).ToArrayAsync(),
                };

                return View(productDTOs);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }

        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDTOs model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Categories = await _context.Categories.Select(a => new SelectListItem
                    {
                        Text = a.categoryName,
                        Value = a.Id.ToString(),
                    }).ToArrayAsync();
                    return View(model);
                }
                Product sales = await _context.Products.FirstOrDefaultAsync(a => a.Id == model.Id);

                if (sales is null)
                {
                    return NotFound();
                }

                Response<Product> response = await _productService.UpdateAsync(model);

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
        public async Task<IActionResult> Delete(CategoriesDTOs model, [FromRoute] int id)
        {
            try
            {
                Product product = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);

                if (product is null)
                {
                    return RedirectToAction(nameof(Index));
                }

                Response<Product> response = await _productService.DeleteAsync(id);


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