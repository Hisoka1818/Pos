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
        public Task<Response<SectionDTO>> GetSectionAsync(PaginationRequest request, int id);

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


        public async Task<Response<SectionDTO>> GetSectionAsync(PaginationRequest request, int id)
        {
            try
            {
                Section? section = await _context.Sections.Include(s => s.RoleSections)
                                                          .Where(s => !s.IsHidden && s.Id == id)
                                                          .FirstOrDefaultAsync();

                if (section is null)
                {
                    return ResponseHelper<SectionDTO>.MakeResponseFail($"La sección con id '{id}' no existe o está oculta.");
                }

                ClaimsUser? claimsUser = _htttpContextAccessor.HttpContext?.User;
                string? userName = claimsUser.Identity.Name;
                User user = await _usersService.GetUserAsync(userName);

                bool isAuthorized = true;
                if (!await _usersService.CurrentUserIsSuperAdmin())
                {
                    isAuthorized = section.RoleSections.Any(rs => rs.RoleId == user.PrivatePosRoleId);
                }

                if (!isAuthorized)
                {
                    return ResponseHelper<SectionDTO>.MakeResponseFail($"No tiene permiso para visualizar esta sección.");
                }

                /* A REVISION
                 
                  
                 IQueryable<Blog> query = _context.Blogs.Where(a => a.Section == section && a.IsPublished);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(q => q.Title.ToLower().Contains(request.Filter.ToLower()));
                }
                query = query.Select(s => new Blog
                {
                    Title = s.Title,
                    Id = s.Id,
                });

                PagedList<Blog> list = await PagedList<Blog>.ToPagedListAsync(query, request);

                PaginationResponse<Blog> result = new PaginationResponse<Blog>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,
                };

                SectionDTO dto = new SectionDTO
                {
                    Id = section.Id,
                    Name = section.Name,
                    PaginatedBlogs = result
                };*/

                return ResponseHelper<SectionDTO>.MakeResponseSuccess(dto);
            }
            catch (Exception ex)
            {
                return ResponseHelper<SectionDTO>.MakeResponseFail(ex);
            }
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
