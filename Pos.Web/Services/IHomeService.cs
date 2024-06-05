using Pos.Web.Core.Pagination;
using Pos.Web.Core;
using Pos.Web.Data.Entities;
using Pos.Web.Data;
using Pos.Web.Helpers;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;
using Pos.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq;
using Pos.Web.Services;
using System.Reflection.Metadata;

namespace PrivateBlog.Web.Services
{
    public interface IHomeService
    {

        public Task<Response<PaginationResponse<Section>>> GetSectionsAsync(PaginationRequest request);
    }

    public class HomeService : IHomeService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _htttpContextAccessor;
        private readonly IUsersService _usersService;

        public HomeService(DataContext context, IHttpContextAccessor htttpContextAccessor, IUsersService usersService)
        {
            _context = context;
            _htttpContextAccessor = htttpContextAccessor;
            _usersService = usersService;
        }



        public async Task<Response<PaginationResponse<Section>>> GetSectionsAsync(PaginationRequest request)
        {
            try
            {
                ClaimsUser? claimsUser = _htttpContextAccessor.HttpContext?.User;
                string? userName = claimsUser.Identity.Name;
                User user = await _usersService.GetUserAsync(userName);

                IQueryable<Section> query = _context.Sections.Where(s => !s.IsHidden);

                if (!await _usersService.CurrentUserIsSuperAdmin())
                {
                    query = query.Where(s => s.RoleSections.Any(rs => rs.RoleId == user.PrivatePosRoleId));
                }

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                query = query.Select(s => new Section
                {
                    Name = s.Name,
                    Id = s.Id,
                });

                PagedList<Section> list = await PagedList<Section>.ToPagedListAsync(query, request);

                PaginationResponse<Section> response = new PaginationResponse<Section>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,
                };

                return ResponseHelper<PaginationResponse<Section>>.MakeResponseSuccess(response);
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Section>>.MakeResponseFail(ex);
            }
        }
    }
}
