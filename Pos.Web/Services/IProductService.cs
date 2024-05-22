using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTOs;
using Pos.Web.Helpers;
using Pos.Web.Core.Pagination;



namespace Pos.Web.Services
{
    public interface IProductService
    {
        public Task<Response<PaginationResponse<Product>>> GetListAsync(PaginationRequest request);

        public Task<Response<List<Product>>> GetListAsync();

        public Task<Response<Product>> CreateAsync(ProductDTOs model);

        public Task<Response<Product>> UpdateAsync(ProductDTOs model);

        Task<Response<Product>> DeleteAsync(int id);

        public class ProductService : IProductService
        {

            private readonly DataContext _context;

            public ProductService(DataContext context)
            {
                _context = context;
            }

            public async Task<Response<Product>> CreateAsync(ProductDTOs model)
            {
                try
                {
                    Product Product = new Product
                    {
                        Name = model.Name,
                        price = model.price,
                        reference = model.reference,
                        area = model.area,
                        Categories = await _context.Categories.FirstOrDefaultAsync(a => a.Id == model.CategoriesId)
                    };

                    await _context.AddAsync(Product);
                    await _context.SaveChangesAsync();


                    return ResponseHelper<Product>.MakeResponseSuccess(Product, "Producto creado con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Product>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<List<Product>>> GetListAsync()
            {
                try
                {
                    List<Product> list = await _context.Product.Include(b => b.Categories).ToListAsync();
                    return ResponseHelper<List<Product>>.MakeResponseSuccess(list, "Producto obtenido con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<List<Product>>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Product>> UpdateAsync(ProductDTOs model)
            {

                try
                {
                    Product product = await _context.Product.FirstOrDefaultAsync(a => a.Id == model.Id);

                    product.Name = model.Name;
                    product.price = model.price;
                    product.reference = model.reference;
                    product.area = model.area;
                    product.Categories = await _context.Categories.FirstOrDefaultAsync(a => a.Id == model.CategoriesId);



                    _context.Product.Update(product);
                    await _context.SaveChangesAsync();


                    return ResponseHelper<Product>.MakeResponseSuccess(product, "Producto Actualizado  con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Product>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<PaginationResponse<Product>>> GetListAsync(PaginationRequest request)
            {
                try
                {
                    IQueryable<Product> queryable = _context.Product.AsQueryable();

                    //Ajustar en este momento se esta buscando por tipo de cliente
                    if (!string.IsNullOrWhiteSpace(request.Filter))
                    {
                        queryable = queryable.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                    }

                    PagedList<Product> list = await PagedList<Product>.ToPagedListAsync(queryable, request);


                    PaginationResponse<Product> result = new PaginationResponse<Product>
                    {
                        List = list,
                        TotalCount = list.TotalCount,
                        RecordsPerPage = list.RecordsPerPage,
                        CurrentPage = list.CurrentPage,
                        TotalPages = list.TotalPages,

                        //Faltante:
                        Filter = request.Filter,
                    };

                    return ResponseHelper<PaginationResponse<Product>>.MakeResponseSuccess(result, "Producto obtenidos con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<PaginationResponse<Product>>.MakeResponseFail(ex);
                }
            }


            public async Task<Response<Product>> DeleteAsync([FromRoute] int id)
            {
                try
                {
                    Product product = await _context.Product.FirstOrDefaultAsync(a => a.Id == id);
                    _context.Product.Remove(product);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<Product>.MakeResponseSuccess(product, "Producto eliminada con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Product>.MakeResponseFail(ex);
                }

            }

        }
    }
}