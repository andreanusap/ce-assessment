using CE.Assessment.BusinessLogic.Services;
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

        [HttpPut("{merchantProductNo}")]
        public async Task<IActionResult> UpdateProductStock(string merchantProductNo)
        {
            try
            {
                var isSuccess = await _productService.UpdateStock(merchantProductNo, 25);
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
