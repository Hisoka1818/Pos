using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pos.Web.Services;

namespace Pos.Web.Core.Attributes
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute(string permission, string module) : base(typeof(CustomAuthorizeFilter))
        {
            Arguments = new object[] { permission, module };
        }
    }

    public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permission;
        private readonly string _module;
        private readonly IUsersService _usersService;

        public CustomAuthorizeFilter(string permission, string module, IUsersService userService)
        {
            _permission = permission;
            _module = module;
            _usersService = userService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool isAuthorized = await _usersService.CurrentUserIsAuthorizedAsync(_permission, _module);

            if (!isAuthorized)
            {
                // Rechazar el acceso si el usuario no tiene el rol requerido.
                context.Result = new ForbidResult();
            }
        }
    }
}