using CE.Assessment.BusinessLogic.Entities;
using CE.Assessment.BusinessLogic.Services;
using CE.Assessment.BusinessLogic.Test.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
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

            var model = TestEntityFactory.CreatePatchResponse(httpStatusCode);

            messageHandlerMock = new Mock<HttpMessageHandler>();
            messageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode,
                    Content = new StringContent(JsonConvert.SerializeObject(model))
                });

            _httpClient = new HttpClient(messageHandlerMock.Object);
            _productService = new ProductService(_httpClient, mockOptions.Object);
        }

        [Fact]
        public async Task ShouldReturnsSuccessPatchResponse_WhenRequestIsSuccess()
        {
            //Arrange
            var merchantProductNo = "1234-S";
            var patchDoc = new JsonPatchDocument();
            patchDoc.Replace("/Stock", 25);

            InitService();

            //Act
            var result = await _productService.UpdateProduct(merchantProductNo, patchDoc);

            //Assert
            result.Should().BeOfType<PatchResponse>();
            result.Success.Should().BeTrue();
            result.StatusCode.Should().Be(200);
        }
    }
}
