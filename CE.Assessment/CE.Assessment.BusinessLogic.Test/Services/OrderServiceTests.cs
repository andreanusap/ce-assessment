﻿using CE.Assessment.BusinessLogic.Entities;
using CE.Assessment.BusinessLogic.Services;
using CE.Assessment.BusinessLogic.Test.Helpers;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using System.Threading.Tasks;

namespace CE.Assessment.BusinessLogic.Test.Services
{
    public class OrderServiceTests
    {
        private readonly IOrderService _orderService;

        public OrderServiceTests() 
        {
            _orderService = new OrderService();   
        }

        [Fact]
        public async Task ShouldReturnTop5Products_WhenOrdersIsProvidedAsync() 
        {
            //Arrange
            var orderDetails = TestEntityFactory.CreateOrderDetails(maxQuantity: 20);

            //Act
            var result = await _orderService.GetTop5OrderedProducts(orderDetails);
            
            var orderProducts = result.ToList();

            //Assert
            result.Should().BeOfType<IEnumerable<OrderProduct>>();
            orderProducts.Should().BeOfType<List<OrderProduct>>();
            orderProducts.Count.Should().Be(5);
            orderProducts.First().TotalQuantity.Should().Be(20);
        }

        [Fact]
        public async Task ShouldReturnNull_WhenOrdersIsNotProvided() 
        {
            //Arrange
            var orderDetails = new List<OrderDetail>();

            //Act
            var result = await _orderService.GetTop5OrderedProducts(orderDetails);

            //Assert
            result.Should().BeNull();
        }
    }
}