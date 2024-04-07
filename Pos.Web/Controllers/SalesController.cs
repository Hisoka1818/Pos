using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Pos.Web.Core;
using Pos.Web.Data.Entities;
using Pos.Web.Services;



namespace Pos.Web.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISalesService _salesService;
        private readonly INotyfService _notify;

        public SalesController(ISalesService salesService, INotyfService notify)
        {
            _salesService = salesService;
            _notify = notify;
        }
    }
}
