using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using Pos.Web.Core;
using Pos.Web.Core.Pagination;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTOs;
using Pos.Web.Helpers;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;


namespace Pos.Web.Services
{
    public interface IUsersService
    {
        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<bool> CheckPasswordAsync(User user, string password);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<Response<User>> CreateAsync(UserDTO dto);

        Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);

        Task<bool> CurrentUserIsSuperAdmin();

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<User?> GetCurrentUserAsync();

        Task<User> GetUserAsync(string email);

        Task<PaginationResponse<User>> GetUsersPaginatedAsync(PaginationRequest request);

        Task<SignInResult> LoginAsync(LoginDTO model);

        Task LogoutAsync();

        Task<IdentityResult> ResetPasswordAsync(User user, string resetToken, string newPassword);

        Task<IdentityResult> UpdateUserAsync(User user);
    }

    public class UsersService : IUsersService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _httpContextAccessor;

        public UsersService(UserManager<User> userManager, DataContext context, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IConverterHelper converterHelper)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _converterHelper = converterHelper;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<Response<User>> CreateAsync(UserDTO dto)
        {
            try
            {
                User user = _converterHelper.ToUser(dto);
                Guid id = Guid.NewGuid();
                user.Id = id.ToString();

                IdentityResult res = await AddUserAsync(user, dto.Document);

                if (!res.Succeeded)
                {
                    return ResponseHelper<User>.MakeResponseFail("Error al crear el usuario.");
                }

                // TODO: Eliminar cuando se haga envío de Email
                string token = await GenerateEmailConfirmationTokenAsync(user);
                await ConfirmEmailAsync(user, token);

                return ResponseHelper<User>.MakeResponseSuccess(user, "Usuario creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<User>.MakeResponseFail(ex);
            }
        }

        public async Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module)
        {
            ClaimsUser? claimUser = _httpContextAccessor.HttpContext?.User;

            // Valida si esta logueado
            if (claimUser is null)
            {
                return false;
            }

            string? userName = claimUser.Identity.Name;

            User? user = await GetUserAsync(userName);

            // Valida si user existe
            if (user is null)
            {
                return false;
            }

            // Valida si es admin
            if (user.PrivatePosRole.Name == "Administrador")
            {
                return true;
            }

            // Si no es administrador, valida si tiene el permiso
            return await _context.Permissions.Include(p => p.RolePermissions)
                                             .AnyAsync(p => (p.Module == module && p.Name == permission)
                                                        && p.RolePermissions.Any(rp => rp.RoleId == user.PrivatePosRoleId));
        }

        public async Task<bool> CurrentUserIsSuperAdmin()
        {
            ClaimsUser? claimUser = _httpContextAccessor.HttpContext?.User;

            // Valida si esta logueado
            if (claimUser is null)
            {
                return false;
            }

            string? userName = claimUser.Identity.Name;

            User? user = await GetUserAsync(userName);

            // Valida si user existe
            if (user is null)
            {
                return false;
            }

            // Valida si es admin
            if (user.PrivatePosRole.Name == Constants.SUPER_ADMIN_ROLE_NAME)
            {
                return true;
            }

            return false;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;

            if (claimsUser is null)
            {
                return null;
            }

            string? userName = claimsUser.Identity.Name;

            User? user = await GetUserAsync(userName);

            return user;
        }

        public async Task<User> GetUserAsync(string email)
        {
            User? user = await _context.Users.Include(u => u.PrivatePosRole)
                                             .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<PaginationResponse<User>> GetUsersPaginatedAsync(PaginationRequest request)
        {
            IQueryable<User> queryable = _context.Users.AsQueryable()
                                                       .Include(u => u.PrivatePosRole);

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                queryable = queryable.Where(q => q.FirstName.ToLower().Contains(request.Filter.ToLower())
                                                || q.LastName.ToLower().Contains(request.Filter.ToLower())
                                                || q.Document.ToLower().Contains(request.Filter.ToLower())
                                                || q.Email.ToLower().Contains(request.Filter.ToLower())
                                                || q.PhoneNumber.ToLower().Contains(request.Filter.ToLower()));
            }

            PagedList<User> list = await PagedList<User>.ToPagedListAsync(queryable, request);

            PaginationResponse<User> result = new PaginationResponse<User>
            {
                List = list,
                TotalCount = list.TotalCount,
                RecordsPerPage = list.RecordsPerPage,
                CurrentPage = list.CurrentPage,
                TotalPages = list.TotalPages,
                Filter = request.Filter
            };

            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginDTO model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public Task<IdentityResult> ResetPasswordAsync(User user, string resetToken, string newPassword)
        {
            return _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
