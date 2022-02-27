using CE.Assessment.Web.Models;
using CE.Assessment.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CE.Assessment.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _homeService.GetOrders(1));
        }

        [HttpPost]
        public async Task<ActionResult> Index(int currentPageIndex)
        {
            return View(await _homeService.GetOrders(currentPageIndex));
        }

        public async Task<IActionResult> TopProducts()
        {
            return View(await _homeService.GetTop5Orders());
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            return View(await _homeService.UpdateOrderedProduct(id, 25));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}