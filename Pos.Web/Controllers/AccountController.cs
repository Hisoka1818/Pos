using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pos.Web.Data.Entities;
using Pos.Web.DTOs;
using Pos.Web.Helpers;
using Pos.Web.Services;

namespace Pos.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConverterHelper _converterHelper;
        private readonly INotyfService _noty;
        private readonly IUsersService _usersService;

        public AccountController(IUsersService usersService, INotyfService noty, IConverterHelper converterHelper)
        {
            _usersService = usersService;
            _noty = noty;
            _converterHelper = converterHelper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _usersService.LoginAsync(dto);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Dashboard", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
                _noty.Error("Email o contraseña incorrectos");
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _usersService.LogoutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult NotAuthorized()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser()
        {
            User? user = await _usersService.GetUserAsync(User.Identity.Name);

            if (user is null)
            {
                return NotFound();
            }

            AccountUserDTO dto = _converterHelper.ToAccountDTO(user);

            return View(dto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(AccountUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                User user = await _usersService.GetUserAsync(User.Identity.Name);

                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PhoneNumber = dto.PhoneNumber;

                await _usersService.UpdateUserAsync(user);

                _noty.Success("Usuario editado con éxito");

                return RedirectToAction("Dashboard", "Home");
            }

            _noty.Error("Debe ajustar los errores de validación.");
            return View(dto);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            if (ModelState.IsValid)
            {
                User user = await _usersService.GetUserAsync(User.Identity.Name);

                bool isCorrectPassword = await _usersService.CheckPasswordAsync(user, dto.CurrentPassword);

                if (!isCorrectPassword)
                {
                    _noty.Error("Contraseña incorrecta");
                    return View();
                }

                string restToken = await _usersService.GeneratePasswordResetTokenAsync(user);
                IdentityResult result = await _usersService.ResetPasswordAsync(user, restToken, dto.NewPassword);

                if (result.Succeeded)
                {
                    _noty.Success("Contraseña actualizada con éxito");
                    return RedirectToAction("Dashboard", "Home");
                }

                _noty.Error("Ha ocurriso un error, intentole nuevamente");
                return View();
            }

            _noty.Error("Debe ajustar los errores de validación");
            return View();
        }
    }
}

