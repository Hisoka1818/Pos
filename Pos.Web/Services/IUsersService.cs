using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pos.Web.Data;
using Pos.Web.Data.Entities;
using Pos.Web.DTOs;

using ClaimsUser = System.Security.Claims.ClaimsPrincipal;


namespace Pos.Web.Services
{
    public interface IUsersService
    {
        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<bool> CheckPasswordAsync(User user, string password);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);

        Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<User> GetUserAsync(string email);

        Task<SignInResult> LoginAsync(LoginDTO model);

        Task LogoutAsync();

        Task<IdentityResult> ResetPasswordAsync(User user, string resetToken, string newPassword);

        Task<IdentityResult> UpdateUserAsync(User user);
    }

    public class UsersService : IUsersService
    {
        private readonly DataContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _httpContextAccessor;

        public UsersService(UserManager<User> userManager, DataContext context, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        //private IHttpContextAccessor _httpContextAccessor;


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

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<User> GetUserAsync(string email)
        {
            User? user = await _context.Users.Include(u => u.PrivatePosRole).FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<SignInResult> LoginAsync(LoginDTO model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string resetToken, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
