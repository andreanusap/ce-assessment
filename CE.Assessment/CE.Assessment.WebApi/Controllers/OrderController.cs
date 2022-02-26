using CE.Assessment.Shared.Entities;
using CE.Assessment.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace CE.Assessment.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        /// <summary>
        /// Get Orders
        /// </summary>
        /// <param name="statuses">List of status</param>
        /// <param name="page">Page</param>
        /// <returns>Order Response</returns>
        [HttpGet]
        public async Task<ActionResult<OrderResponse>> GetOrders([FromQuery]string[] statuses, [FromQuery]int page)
        {
            try
            {
                var result = await _orderService.GetOrders(statuses, page);

                if(result is null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get orders");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get Top 5 Ordered Products
        /// </summary>
        /// <returns>List of Ordered products</returns>
        [HttpGet("top5-ordered")]
        public async Task<ActionResult<IEnumerable<OrderProduct>>> GetTop5OrderedProducts()
        {
            try
            {
                var orders = await _orderService.GetInProgressOrders();
                var result = await _orderService.GetTop5OrderedProducts(orders);

                if (result is null || !result.Any())
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get top 5 ordered products");
                return StatusCode(500);
            }
        }
    }
}
