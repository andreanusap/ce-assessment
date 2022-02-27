using CE.Assessment.BusinessLogic.Helpers;
using CE.Assessment.BusinessLogic.Services;
using CE.Assessment.BusinessLogic.Test.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CE.Assessment.BusinessLogic.Test.Services
{
    public class ProductServiceTests
    {
        private IProductService _productService;
        private Mock<IHttpClientHelper> mockHttpClientHelper;
        private Mock<ILogger<ProductService>> mockLogger;

        [Fact]
        public async Task ShouldReturnTrue_WhenRequestIsSuccess()
        {
            //Arrange
            var merchantProductNo = "1234-S";
            var stock = 25;

            mockHttpClientHelper = new Mock<IHttpClientHelper>();
            mockLogger = new Mock<ILogger<ProductService>>();

            mockHttpClientHelper
                .Setup(s => s.HttpPut(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            _productService = new ProductService(mockHttpClientHelper.Object, mockLogger.Object);

            //Act
            var result = await _productService.UpdateStock(merchantProductNo, stock);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnFalse_WhenRequestIsFailed()
        {
            //Arrange
            var merchantProductNo = "1234-S";
            var stock = 25;

            mockHttpClientHelper = new Mock<IHttpClientHelper>();
            mockLogger = new Mock<ILogger<ProductService>>();

            mockHttpClientHelper
                .Setup(s => s.HttpPut(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            _productService = new ProductService(mockHttpClientHelper.Object, mockLogger.Object);

            //Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _productService.UpdateStock(It.IsAny<string>(), It.IsAny<int>()));
            mockLogger.VerifyAtLeastOneLogMessagesContains("Update Stock service failed");
        }
    }
}
