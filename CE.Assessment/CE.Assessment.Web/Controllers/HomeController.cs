using AutoMapper;
using CE.Assessment.BusinessLogic.Entities;
using CE.Assessment.BusinessLogic.Services;
using CE.Assessment.Web.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CE.Assessment.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger,
            IOrderService orderService,
            IProductService productService,
            IMapper mapper)
        {
            _logger = logger;
            _orderService = orderService;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var orderDetails = await _orderService.GetInProgressOrders();

            var model = _mapper.Map<List<OrderDetailModel>>(orderDetails);

            return View(model);
        }

        public async Task<IActionResult> TopProducts()
        {
            var orderDetails = await _orderService.GetInProgressOrders();
            var topProducts = await _orderService.GetTop5OrderedProducts(orderDetails);
            return View(topProducts);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var patchDoc = new JsonPatchDocument();
            patchDoc.Replace("/Stock", 25);
            var result = await _productService.UpdateProduct(id, patchDoc);
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}