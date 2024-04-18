using Microsoft.EntityFrameworkCore;
using Pos.Web.Core;
using Pos.Web.Core.Pagination;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.Helpers;


namespace Pos.Web.Services
{
    public interface ICustomerService
    {
        public Task<Response<PaginationResponse<Customer>>> GetListAsync(PaginationRequest request);
        public Task<Response<Customer>> CreateAsync(Customer model);
        public Task<Response<Customer>> GetOneAsync(int id);
        public Task<Response<Customer>> EditAsync(Customer model);
        public Task<Response<Customer>> DeleteAsync(int id);

        public class CustomerService : ICustomerService
        {
            private readonly DataContext _context;

            public CustomerService(DataContext context)
            {
                _context = context;
            }

            public async Task<Response<Customer>> CreateAsync(Customer model)
            {
                try
                {
                    Customer customer = new Customer
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        EmailAddress = model.EmailAddress,
                        CustomerType = model.CustomerType,
                    };

                    await _context.AddAsync(customer);
                    await _context.SaveChangesAsync();


                    return ResponseHelper<Customer>.MakeResponseSuccess(customer, "Cliente creado con exito");
                }catch (Exception ex)
                {
                    return ResponseHelper<Customer>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Customer>> DeleteAsync(int id)
            {
                try
                {
                    Customer? customer = await _context.Customer.FirstOrDefaultAsync(s => s.Id == id);

                    if (customer is null)
                    {
                        return ResponseHelper<Customer>.MakeResponseFail($"El cliente con id '{id}' no existe.");
                    }
                    
                    _context.Customer.Remove(customer);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<Customer>.MakeResponseSuccess("Cliente eliminado con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<Customer>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Customer>> EditAsync(Customer model)
            {
                try
                {
                    _context.Customer.Update(model);
                    await _context.SaveChangesAsync();

                    return ResponseHelper<Customer>.MakeResponseSuccess(model, "Cliente actualizado con éxito");

                }
                catch (Exception ex)
                {
                    return ResponseHelper<Customer>.MakeResponseFail(ex);
                }
            }

            public async Task<Response<Customer>> GetOneAsync(int id)
            {
                try
                {
                    Customer? customer = await _context.Customer.FirstOrDefaultAsync(s => s.Id == id);

                    if (customer is null)
                    {
                        return ResponseHelper<Customer>.MakeResponseFail($"El cliente con id '{id}' no existe.");
                    }

                    return ResponseHelper<Customer>.MakeResponseSuccess(customer);

                }
                catch (Exception ex)
                {
                    return ResponseHelper<Customer>.MakeResponseFail(ex);

                }
            }

            public async Task<Response<PaginationResponse<Customer>>> GetListAsync(PaginationRequest request)
            {
                try
                {
                    IQueryable<Customer> queryable = _context.Customer.AsQueryable();

                    //Ajustar en este momento se esta buscando por tipo de cliente
                    if (!string.IsNullOrWhiteSpace(request.Filter))
                    {
                        queryable = queryable.Where(s => s.CustomerType.ToLower().Contains(request.Filter.ToLower()));
                    }

                    PagedList<Customer> list = await PagedList<Customer>.ToPagedListAsync(queryable, request);


                    PaginationResponse<Customer> result = new PaginationResponse<Customer>
                    {
                        List = list,
                        TotalCount = list.TotalCount,
                        RecordsPerPage = list.RecordsPerPage,
                        CurrentPage = list.CurrentPage,
                        TotalPages = list.TotalPages,

                        //Faltante:
                        Filter = request.Filter,
                    };

                    return ResponseHelper<PaginationResponse<Customer>>.MakeResponseSuccess(result, "Clientes obtenidos con éxito");
                }
                catch (Exception ex)
                {
                    return ResponseHelper<PaginationResponse<Customer>>.MakeResponseFail(ex);
                }
            }
        }

    }
}
