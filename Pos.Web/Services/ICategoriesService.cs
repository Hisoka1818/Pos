using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.Helpers;


namespace Pos.Web.Services
{
    public interface ICategoriesService
    {
        public Task<Response<List<Categories>>> GetListAsync();
        public Task<Response<Categories>> CreateAsync(Categories model);
        public Task<Response<Categories>> GetOneAsync(int id);
        public Task<Response<Categories>> EditAsync(Categories model);
        public Task<Response<Categories>> DeleteAsync(int id);

        public class CategoriesService : ICategoriesService
        {
            private readonly DataContext _context;

            public CategoriesService(DataContext context)
            {
                _context = context;
            }

            public async Task<Response<Categories>> CreateAsync(Categories model)
            {
                try
                {
                    Categories categories = new Categories
                    {
                        categoryName = model.categoryName,
                        categoryDescription = model.categoryDescription,

                    };

                    await _context.AddAsync(categories);
                    await _context.SaveChangesAsync();


                    return ResponseHelper<Categories>.MakeResponseSuccess(categories, "Categoría creada exitosamente");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Categories>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Categories>> DeleteAsync(int id)
            {
                try
                {
                    Categories? categories = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);

                    if (categories is null)
                    {
                        return ResponseHelper<Categories>.MakeResponseFail($"La categoría con id '{id}' no existe.");
                    }

                    _context.Categories.Remove(categories);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<Categories>.MakeResponseSuccess("Categoría eliminada con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Categories>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Categories>> EditAsync(Categories model)
            {
                try
                {
                    _context.Categories.Update(model);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<Categories>.MakeResponseSuccess(model, "Categoría actualizada con éxito");

                }
                catch (Exception ex)
                {
                    return ResponseHelper<Categories>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<List<Categories>>> GetListAsync()
            {
                try
                {
                    List<Categories> list = await _context.Categories.ToListAsync();
                    return ResponseHelper<List<Categories>>.MakeResponseSuccess(list, "Categoría obtenida con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<List<Categories>>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Categories>> GetOneAsync(int id)
            {
                try
                {
                    Categories? categories = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);

                    if (categories is null)
                    {
                        return ResponseHelper<Categories>.MakeResponseFail($"La categoría con id '{id}' no existe.");
                    }

                    return ResponseHelper<Categories>.MakeResponseSuccess(categories);

                }
                catch (Exception ex)
                {
                    return ResponseHelper<Categories>.MakeResponseFail(ex);

                }
            }
        }

    }

}   


