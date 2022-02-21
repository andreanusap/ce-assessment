using AutoMapper;
using CE.Assessment.BusinessLogic.Services;
using CE.Assessment.Web.Models;
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
            return View(await GetOrders(1));
        }

        [HttpPost]
        public async Task<ActionResult> Index(int currentPageIndex)
        {
            return View(await GetOrders(currentPageIndex));
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

            var result = await _productService.UpdateStock(id, 25);
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<OrderViewModel> GetOrders(int page)
        {
            var statuses = new string[] { "IN_PROGRESS" };
            
            var orderDetails = await _orderService.GetOrders(statuses: statuses, page: page);

            if(orderDetails is not null && orderDetails.Count > 0)
            {
                double pageCount = orderDetails.TotalCount / orderDetails.Count;
                var model = _mapper.Map<OrderViewModel>(orderDetails);
                model.CurrentPage = page;
                model.PageCount = (int)Math.Ceiling(pageCount);
                return model;
            }
            else
            {
                return _mapper.Map<OrderViewModel>(orderDetails);
            }
        }
    }
}