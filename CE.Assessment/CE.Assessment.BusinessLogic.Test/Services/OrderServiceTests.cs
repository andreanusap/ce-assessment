using CE.Assessment.BusinessLogic.Services;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using CE.Assessment.BusinessLogic.Helpers;
using Microsoft.Extensions.Logging;
using System;
using CE.Assessment.BusinessLogic.Test.Extensions;
using CE.Assessment.Shared.Entities;

namespace CE.Assessment.BusinessLogic.Test.Services
{
    public class OrderServiceTests
    {
        private IOrderService _orderService;
        private Mock<IHttpClientHelper> mockHttpClientHelper;
        private Mock<ILogger<OrderService>> mockLogger;

        [Fact]
        public async Task ShouldReturnOrderDetails_WhenGetInProgressOrdersSuccess()
        {
            //Arrange
            var orderDetails = TestEntityFactory.CreateOrderDetails();
            var content = new OrderResponse
            {
                Content = orderDetails.ToList()
            };

            mockHttpClientHelper = new Mock<IHttpClientHelper>();
            mockLogger = new Mock<ILogger<OrderService>>();

            mockHttpClientHelper
                .Setup(s => s.HttpGet<OrderResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(content);

            _orderService = new OrderService(mockHttpClientHelper.Object, mockLogger.Object);

            //Act
            var result = await _orderService.GetInProgressOrders();

            //Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ShouldThrowException_WhenGetInProgressOrdersSuccess()
        {
            //Arrange
            var orderDetails = TestEntityFactory.CreateOrderDetails();
            var content = new OrderResponse
            {
                Content = orderDetails.ToList()
            };

            mockHttpClientHelper = new Mock<IHttpClientHelper>();
            mockLogger = new Mock<ILogger<OrderService>>();

            mockHttpClientHelper
                .Setup(s => s.HttpGet<OrderResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            _orderService = new OrderService(mockHttpClientHelper.Object, mockLogger.Object);

            //Act
            var result = await _orderService.GetInProgressOrders();

            //Assert
            result.Should().BeNull();
            mockLogger.VerifyAtLeastOneLogMessagesContains("Get In Progress service failed");
        }

        [Fact]
        public async Task ShouldReturnTop5Products_WhenOrdersIsProvidedAsync() 
        {
            //Arrange
            var orderDetails = TestEntityFactory.CreateOrderDetails(maxQuantity: 20);
            mockHttpClientHelper = new Mock<IHttpClientHelper>();
            mockLogger = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(mockHttpClientHelper.Object, mockLogger.Object);

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
            mockHttpClientHelper = new Mock<IHttpClientHelper>();
            mockLogger = new Mock<ILogger<OrderService>>();
            _orderService = new OrderService(mockHttpClientHelper.Object, mockLogger.Object);

            //Act
            var result = await _orderService.GetTop5OrderedProducts(orderDetails);

            //Assert
            result.Should().BeEmpty();
        }
    }
}
