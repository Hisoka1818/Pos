using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTO;
using Pos.Web.Helpers;
using static Pos.Web.Services.IProductsService;

namespace Pos.Web.Services
{
    public interface IProductsService
    {
        public Task<Response<List<Products>>> GetListAsync();
        public Task<Response<Products>> CreateAsync(ProductsDTO model);

        public Task<Response<Products>> UpdateAsync(ProductsDTO model);

        Task<Response<Products>> DeleteAsync(int id);

        public class ProductsService : IProductsService
        {

            private readonly DataContext _context;

            public ProductsService(DataContext context)
            {
                _context = context;
            }

            public async Task<Response<Products>> CreateAsync(ProductsDTO model)
            {
                try
                {
                    Products Products = new Products
                    {
                        Name = model.Name,
                        price = model.price,
                        reference = model.reference,
                        area = model.area,
                        Categories = await _context.Categories.FirstOrDefaultAsync(a => a.Id == model.CategoriesId)
                    };

                    await _context.AddAsync(Products);
                    await _context.SaveChangesAsync();


                    return ResponseHelper<Products>.MakeResponseSuccess(Products, "Producto creado con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Products>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<List<Products>>> GetListAsync()
            {
                try
                {
                    List<Products> list = await _context.Products.Include(b => b.Categories).ToListAsync();
                    return ResponseHelper<List<Products>>.MakeResponseSuccess(list, "Producto obtenido con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<List<Products>>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Products>> UpdateAsync(ProductsDTO model)
            {

                try
                {
                    Products products = await _context.Products.FirstOrDefaultAsync(a => a.Id == model.Id);
                    
                    products.Name = model.Name;
                    products.price = model.price;
                    products.reference = model.reference;
                    products.area = model.area;
                    products.Categories = await _context.Categories.FirstOrDefaultAsync(a => a.Id == model.CategoriesId);



                   _context.Products.Update(products);
                    await _context.SaveChangesAsync();


                    return ResponseHelper<Products>.MakeResponseSuccess(products, "Producto Actualizado  con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Products>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Products>> DeleteAsync([FromRoute] int id)
            {
                try
                {
                    Products products = await _context.Products.FirstOrDefaultAsync(a => a.Id == id);
                    _context.Products.Remove(products);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<Products>.MakeResponseSuccess(products, "Producto eliminada con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Products>.MakeResponseFail(ex);
                }

            }

        }
    }
}