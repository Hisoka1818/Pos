using System.Reflection.PortableExecutable;
using Pos.Web.Core;
using Pos.Web.Core.Pagination;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.Helpers;

namespace Pos.Web.Services
{
    public interface IRolesService
    {
        public Task<Response<PaginationRequest<PrivatePosRole>>> GetListAsync(PaginationRequest request);
    }

    public class RolesService : IRolesService
    {
        private readonly DataContext _context;

        public RolesService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<PaginationRequest<PrivatePosRole>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<PrivatePosRole>queryable=_context.PrivatePosRoles.AsQueryable();
                if(!string.IsNullOrWhiteSpace(request.Filter))
                {
                    queryable = queryable.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<PrivatePosRole> list = await PagedList<PrivatePosRole>.ToPagedListAsync(queryable, request);

                PaginationResponse<PrivatePosRole> result = new PaginationResponse<PrivatePosRole>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,
                };
            }
            catch(Exception ex)
            {
                return ResponseHelper<PaginationRequest<PrivatePosRole>>.MakeResponseFail(ex);
            }
        }
    }
}