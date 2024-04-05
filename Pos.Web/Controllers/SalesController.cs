using Microsoft.AspNetCore.Mvc;

namespace Pos.Web.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
