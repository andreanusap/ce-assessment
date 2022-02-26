﻿using AutoMapper;
using CE.Assessment.Shared.Entities;
using CE.Assessment.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace CE.Assessment.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOptions<ApiOptions> _options;

        public HomeController(IMapper mapper, IOptions<ApiOptions> options)
        {
            _mapper = mapper;
            _options = options;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetOrders(1));
        }

        [HttpPost]
        public async Task<ActionResult> Index(int currentPageIndex)
        {
            return View(await GetOrders(currentPageIndex));
        }

        public async Task<IActionResult> TopProducts()
        {
            var topProducts = new List<OrderProductViewModel>();

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.Value.BaseUrl);
                var responseMessage = await client.GetAsync("order/top5-ordered");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var content = await responseMessage.Content.ReadAsAsync<IEnumerable<OrderProductViewModel>>();

                    if (content is not null && content.Any())
                    {
                        topProducts = content.ToList();
                    }
                }
            }

            return View(topProducts);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            bool result = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.Value.BaseUrl);
                
                var productStockRequest = new ProductStockRequest
                {
                    MerchantProductNo = id,
                    Stock = 25
                };

                var requestJson = JsonConvert.SerializeObject(productStockRequest);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var responseMessage = await client.PutAsync($"product", content);

                if (responseMessage.IsSuccessStatusCode)
                {
                    result = await responseMessage.Content.ReadAsAsync<bool>();
                }
            }
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<OrderViewModel> GetOrders(int page)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.Value.BaseUrl);
                var responseMessage = await client.GetAsync("order");

                if (responseMessage.IsSuccessStatusCode)
                {
                    var orderDetails = await responseMessage.Content.ReadAsAsync<OrderResponse>();

                    if (orderDetails is not null && orderDetails.Count > 0)
                    {
                        double pageCount = orderDetails.TotalCount / orderDetails.Count;
                        var model = _mapper.Map<OrderViewModel>(orderDetails);
                        model.CurrentPage = page;
                        model.PageCount = (int)Math.Ceiling(pageCount);
                        return model;
                    }
                    else
                    {
                        return _mapper.Map<OrderViewModel>(orderDetails);
                    }
                }
                else
                {
                    return new OrderViewModel();
                }
            }
        }
    }
}