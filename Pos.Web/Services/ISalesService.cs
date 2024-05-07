using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTOs;
using Pos.Web.DTO;
using Pos.Web.Helpers;


namespace Pos.Web.Services
{
    public interface ISalesService
    {
        public Task<Response<List<Sales>>> GetListAsync();

        public Task<Response<Sales>> CreateAsync(SalesDTO model);

        public Task<Response<Sales>> UpdateAsync(SalesDTO model);

        public Task<Response<Sales>> DeleteAsync([FromRoute] int id);

        public class SalesService : ISalesService
        {
            private readonly DataContext _context;

            public SalesService(DataContext context)
            {
                _context = context;
            }

            public async Task<Response<Sales>> CreateAsync(SalesDTO model)
            {
                try
                {
                    Sales sales = new Sales
                    {
                        SalesType = model.SalesType,
                        DateSales = model.DateSales,
                        PaymentMethod = model.PaymentMethod,
                        DiscountsSales = model.DiscountsSales,
                        TotalSales = model.TotalSales,
                        Customer = await _context.Customer.FirstOrDefaultAsync(a => a.Id == model.CustomerId)
                    };

                    await _context.AddAsync(sales);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<Sales>.MakeResponseSuccess(sales, "Venta registrada con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Sales>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Sales>> DeleteAsync([FromRoute] int id)
            {
                try
                {
                    Sales sales = await _context.Sales.FirstOrDefaultAsync(a => a.Id == id);
                    _context.Sales.Remove(sales);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<Sales>.MakeResponseSuccess(sales, "Venta eliminada con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Sales>.MakeResponseFail(ex);
                }
               
            }

            public async Task<Response<List<Sales>>> GetListAsync()
            {
                try
                {
                    List<Sales> list = await _context.Sales.Include(b => b.Customer).ToListAsync();
                    return ResponseHelper<List<Sales>>.MakeResponseSuccess(list, "Venta obtenido con exito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<List<Sales>>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Sales>> UpdateAsync(SalesDTO model)
            {
                try
                {
                    Sales sales = await _context.Sales.FirstOrDefaultAsync(a => a.Id == model.Id);

                    sales.DateSales = model.DateSales;
                    sales.SalesType = model.SalesType;
                    sales.PaymentMethod = model.PaymentMethod;
                    sales.TotalSales = model.TotalSales;
                    sales.DiscountsSales = model.DiscountsSales;
                    sales.Customer = await _context.Customer.FirstOrDefaultAsync(a => a.Id == model.CustomerId);

                    _context.Sales.Update(sales);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<Sales>.MakeResponseSuccess(sales, "Venta actualizada con exito");

                }
                catch (Exception ex)
                {
                    return ResponseHelper<Sales>.MakeResponseFail(ex);
                }
            }
        }
    }
}
