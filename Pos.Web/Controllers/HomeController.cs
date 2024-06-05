using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Web.Core.Pagination;
using Pos.Web.Core;
using Pos.Web.DTOs;
using Pos.Web.Models;
using PrivateBlog.Web.Services;
using System.Diagnostics;
using Pos.Web.Data.Entities;

namespace Pos.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

       

        public async Task<IActionResult> Dashboard([FromQuery] int? RecordsPerPage,
                                      [FromQuery] int? Page,
                                      [FromQuery] string? Filter)
        {
            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter
            };

            Response<PaginationResponse<Section>> response = await _homeService.GetSectionsAsync(request);

            return View(response.Result);
        }

        

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
