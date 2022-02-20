using AutoFixture;
using CE.Assessment.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Net;

namespace CE.Assessment.BusinessLogic.Test.Helpers
{
    public class TestEntityFactory
    {
        static readonly Fixture Fixture;

        static TestEntityFactory()
        {
            Fixture = new Fixture();
        }

        /// <summary>
        /// Create mock of line
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <returns>Line</returns>
        public static Line CreateLine(int quantity = 1)
        {
            Random r = new Random();
            var productNo = "ProductNo-"+r.Next(10).ToString();
            return Fixture.Build<Line>()
                .With(x => x.Quantity, quantity)
                .With(x => x.MerchantProductNo, productNo)
                .Create();
        }

        /// <summary>
        /// Create mock of order detail
        /// </summary>
        /// <param name="productQuantity">product's quantity</param>
        /// <returns>Order detail</returns>
        public static OrderDetail CreateOrderDetail(int productQuantity = 1)
        {
            var lines = new List<Line>() { CreateLine(productQuantity) };
            return Fixture.Build<OrderDetail>()
                .With(x => x.Lines, lines)
                .Create();
        }

        /// <summary>
        /// Create mock of Order Details List
        /// </summary>
        /// <param name="maxQuantity">Max product's quantity</param>
        /// <param name="minQuantity">Min product's quantity</param>
        /// <returns>List of order detail</returns>
        public static IEnumerable<OrderDetail> CreateOrderDetails(int maxQuantity = 1, int minQuantity = 1)
        {
            var orders = new List<OrderDetail>();

            for(int i = 0; i < 10; i++) ///create 10 mocks of order detail
            {
                Random r = new Random();
                int rQuantity = r.Next(minQuantity, maxQuantity);
                orders.Add(CreateOrderDetail(rQuantity));
            }

            return orders;
        }

        /// <summary>
        /// Create mock of patch response
        /// </summary>
        /// <param name="httpStatusCode">Http status code</param>
        /// <returns>Patch response</returns>
        public static PatchResponse CreatePatchResponse(HttpStatusCode httpStatusCode)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.OK:
                    return Fixture.Build<PatchResponse>()
                        .With(x => x.Success, true)
                        .With(x => x.StatusCode, 200)
                        .Create();
                case HttpStatusCode.InternalServerError:
                    return Fixture.Build<PatchResponse>()
                        .With(x => x.Success, false)
                        .With(x => x.StatusCode, 500)
                        .Create();
                default:
                    return Fixture.Create<PatchResponse>();
            }
        }
    }
}
