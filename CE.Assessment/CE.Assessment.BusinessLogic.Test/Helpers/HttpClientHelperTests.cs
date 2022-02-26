using CE.Assessment.BusinessLogic.Entities;
using CE.Assessment.BusinessLogic.Helpers;
using CE.Assessment.BusinessLogic.Test.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Options = CE.Assessment.BusinessLogic.Entities.Options;

namespace CE.Assessment.BusinessLogic.Test.Helpers
{
    public class HttpClientHelperTests
    {
        private HttpClient _httpClient;
        private Mock<IOptions<Options>> mockOptions;
        private Mock<ILogger<HttpClientHelper>> mockLogger;
        private IHttpClientHelper _httpClientHelper;
        private Mock<HttpMessageHandler> messageHandlerMock;

        private void InitHttpClientHelper(HttpResponseMessage httpResponseMessage)
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
                .ReturnsAsync(httpResponseMessage);

            _httpClient = new HttpClient(messageHandlerMock.Object);
            mockLogger = new Mock<ILogger<HttpClientHelper>>();
            _httpClientHelper = new HttpClientHelper(_httpClient, mockOptions.Object, mockLogger.Object);
        }

        [Fact]
        public async Task ShouldReturnEntities_WhenHttpGetReturnsSuccess()
        {
            //Arrange
            var orderDetails = TestEntityFactory.CreateOrderDetails();
            var orderResponse = new OrderResponse
            {
                Content = orderDetails.ToList(),
                Count = orderDetails.Count(),
                TotalCount = orderDetails.Count()
            };

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(orderResponse))
            };

            InitHttpClientHelper(httpResponseMessage);

            //Act
            var result = await _httpClientHelper.HttpGet<OrderResponse>("order");

            //Assert
            result.Should().BeOfType<OrderResponse>();
            result.Count.Should().Be(orderResponse.Count);
            messageHandlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task ShouldReturnNull_WhenHttpGetReturnsFailure()
        {
            //Arrange
            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = null
            };

            InitHttpClientHelper(httpResponseMessage);

            //Act
            var result = await _httpClientHelper.HttpGet<OrderResponse>("order");

            //Assert
            result.Should().BeNull();
            messageHandlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task ShouldReturnTrue_WhenHttpPutReturnsSuccess()
        {
            //Arrange
            var productStockRequest = TestEntityFactory.CreateProductStockRequests();
            var serializedDoc = JsonConvert.SerializeObject(productStockRequest);

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            InitHttpClientHelper(httpResponseMessage);

            //Act
            var result = await _httpClientHelper.HttpPut($"offer/stock", serializedDoc);

            //Assert
            result.Should().BeTrue();
            messageHandlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put),
               ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task ShouldReturnFalse_WhenHttpPutReturnsFailure()
        {
            //Arrange
            var productStockRequest = TestEntityFactory.CreateProductStockRequests();
            var serializedDoc = JsonConvert.SerializeObject(productStockRequest);

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            InitHttpClientHelper(httpResponseMessage);

            //Act
            var result = await _httpClientHelper.HttpPut($"offer/stock", serializedDoc);

            //Assert
            result.Should().BeFalse();
            messageHandlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put),
               ItExpr.IsAny<CancellationToken>());
        }
    }
}
