using CE.Assessment.BusinessLogic.Services;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Options = CE.Assessment.BusinessLogic.Entities.Options;

namespace CE.Assessment.BusinessLogic.Test.Services
{
    public class ProductServiceTests
    {
        private IProductService _productService;
        private HttpClient _httpClient;
        private Mock<IOptions<Options>> mockOptions;
        private Mock<HttpMessageHandler> messageHandlerMock;

        public ProductServiceTests() { }

        private void InitService(HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            mockOptions = new Mock<IOptions<Options>>();
            mockOptions.Setup(o => o.Value)
                .Returns(new Options
                {
                    BaseUrl = "https://test.com",
                    ApiKey = "apiKey"
                });

            messageHandlerMock = new Mock<HttpMessageHandler>();
            messageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode
                });

            _httpClient = new HttpClient(messageHandlerMock.Object);
            _productService = new ProductService(_httpClient, mockOptions.Object);
        }

        [Fact]
        public async Task ShouldReturnsSuccess_WhenRequestIsSuccess()
        {
            //Arrange
            var merchantProductNo = "1234-S";
            var stock = 25;

            InitService();

            //Act
            var result = await _productService.UpdateStock(merchantProductNo, stock);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldReturnsFailed_WhenRequestIsNotSuccess()
        {
            //Arrange
            var merchantProductNo = "1234-S";
            var stock = 25;

            InitService(HttpStatusCode.InternalServerError);

            //Act
            var result = await _productService.UpdateStock(merchantProductNo, stock);

            //Assert
            result.Should().BeFalse();
        }
    }
}
