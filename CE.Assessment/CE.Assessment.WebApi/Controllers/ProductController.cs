using CE.Assessment.BusinessLogic.Services;
using CE.Assessment.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CE.Assessment.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        /// <summary>
        /// Update Product Stock
        /// </summary>
        /// <param name="productStockRequest">Updated product data</param>
        /// <returns>Success or failures of update product's stock process</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProductStock([FromBody]ProductStockRequest productStockRequest)
        {
            try
            {
                if(productStockRequest is null 
                    || string.IsNullOrWhiteSpace(productStockRequest.MerchantProductNo)
                    || productStockRequest.Stock < 0)
                {
                    return BadRequest();
                }

                var isSuccess = await _productService.UpdateStock(productStockRequest.MerchantProductNo, 
                    productStockRequest.Stock);
                return isSuccess? Ok(isSuccess) : StatusCode(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update product's stock");
                return StatusCode(500);
            }
        }
    }
}
