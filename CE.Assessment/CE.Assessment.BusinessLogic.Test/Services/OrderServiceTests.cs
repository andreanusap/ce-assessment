using CE.Assessment.BusinessLogic.Entities;
using CE.Assessment.BusinessLogic.Services;
using CE.Assessment.BusinessLogic.Test.Helpers;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using System.Net.Http;
using Moq.Protected;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Options = CE.Assessment.BusinessLogic.Entities.Options;

namespace CE.Assessment.BusinessLogic.Test.Services
{
    public class OrderServiceTests
    {
        private IOrderService _orderService;
        private HttpClient _httpClient;
        private Mock<IOptions<Options>> mockOptions;

        public OrderServiceTests() 
        {
        }

        private void InitService(IEnumerable<OrderDetail> orderDetailList = null,
            OrderResponse orderResponse = null,
            HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            mockOptions = new Mock<IOptions<Options>>();
            mockOptions.Setup(o => o.Value)
                .Returns(new Options
                {
                    BaseUrl = "https://test.com",
                    ApiKey = "apiKey"
                });

            object responseModel = null;

            if (orderDetailList is not null)
            {
                responseModel = new
                {
                    Content = orderDetailList
                };
            }
            else if(orderResponse is not null)
            {
                responseModel = orderResponse;
            }

            var messageHandlerMock = new Mock<HttpMessageHandler>();
            messageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = httpStatusCode,
                    Content = new StringContent(JsonConvert.SerializeObject(responseModel))
                });

            _httpClient = new HttpClient(messageHandlerMock.Object);
            _orderService = new OrderService(_httpClient, mockOptions.Object);
        }

        [Fact]
        public async Task ShouldReturnOrderDetails_WhenGetInProgressOrdersSuccess()
        {
            //Arrange
            var orderDetails = TestEntityFactory.CreateOrderDetails();
            InitService(orderDetailList: orderDetails, httpStatusCode: HttpStatusCode.OK);

            //Act
            var result = await _orderService.GetInProgressOrders();

            //Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ShouldReturnEmpty_WhenWhenGetInProgressOrdersFails()
        {
            //Arrange
            InitService(httpStatusCode: HttpStatusCode.InternalServerError);

            //Act
            var result = await _orderService.GetInProgressOrders();

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnTop5Products_WhenOrdersIsProvidedAsync() 
        {
            //Arrange
            var orderDetails = TestEntityFactory.CreateOrderDetails(maxQuantity: 20);
            InitService(orderDetailList: orderDetails);

            //Act
            var result = await _orderService.GetTop5OrderedProducts(orderDetails);
            
            var orderProducts = result.ToList();
            var maxQuantity = orderProducts.Max(x => x.TotalQuantity);

            //Assert
            orderProducts.Should().BeOfType<List<OrderProduct>>();
            orderProducts.Count.Should().Be(5);
            orderProducts.First().TotalQuantity.Should().Be(maxQuantity);
        }

        [Fact]
        public async Task ShouldReturnEmpty_WhenOrdersIsNotProvided() 
        {
            //Arrange
            var orderDetails = new List<OrderDetail>();
            InitService(orderDetailList: orderDetails);

            //Act
            var result = await _orderService.GetTop5OrderedProducts(orderDetails);

            //Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldReturnOrderResponse_WhenGetOrdersSuccess()
        {
            //Arrange
            var orderDetails = TestEntityFactory.CreateOrderDetails();
            var orderResponse = new OrderResponse()
            {
                Content = orderDetails.ToList(),
                Count = 1
            };
            InitService(orderResponse: orderResponse, httpStatusCode: HttpStatusCode.OK);

            //Act
            var result = await _orderService.GetOrders(It.IsAny<string[]>(), It.IsAny<int>());

            //Assert
            result.Should().BeOfType<OrderResponse>();
            result.Content.Should().NotBeNullOrEmpty();
            result.Count.Should().BeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task ShouldReturnEmpty_WhenGetOrdersFails()
        {
            //Arrange
            InitService(httpStatusCode: HttpStatusCode.InternalServerError);

            //Act
            var result = await _orderService.GetOrders(It.IsAny<string[]>(), It.IsAny<int>());

            //Assert
            result.Should().BeOfType<OrderResponse>();
            result.Content.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }
    }
}
