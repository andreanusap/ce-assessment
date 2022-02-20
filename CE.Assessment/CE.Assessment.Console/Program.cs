using CE.Assessment.BusinessLogic.Entities;
using CE.Assessment.BusinessLogic.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Options = CE.Assessment.BusinessLogic.Entities.Options;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting the app...");
        Console.WriteLine();

        var services = new ServiceCollection();
        ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();

        //Get IN PROGRESS orders
        var orderService = serviceProvider.GetService<IOrderService>();
        var result = orderService.GetInProgressOrders().GetAwaiter().GetResult();
        Console.WriteLine("IN PROGRESS Orders");
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

        //Get Top 5 Ordered Products
        var top5 = orderService.GetTop5OrderedProducts(result).GetAwaiter().GetResult();
        Console.WriteLine();
        Console.WriteLine("Top 5 Ordered Products");
        for(int i = 0; i < 5; i++)
        {
            Console.WriteLine($"{i + 1}. Product Name = {top5.ElementAt(i).ProductName} || " +
                $"GTIN = {top5.ElementAt(i).Gtin} || " +
                $"Qty = {top5.ElementAt(i).TotalQuantity}");
        }

        //Update stock of selected product
        Console.WriteLine();
        Console.WriteLine("Please Select Product to update (1-5):");
        string productNo = Console.ReadLine();
        bool isNumber = Int32.TryParse(productNo, out var number);
        if(isNumber && 1<= number && number <= 5)
        {
            var merchatProductNo = top5.ElementAt(number - 1).MerchantProductNo;
            if (!string.IsNullOrWhiteSpace(merchatProductNo))
            {
                var productService = serviceProvider.GetService<IProductService>();
                var patchDoc = new JsonPatchDocument();
                patchDoc.Replace("/Stock", 25);
                var response = productService.UpdateProduct(merchatProductNo, patchDoc).GetAwaiter().GetResult();
                Console.WriteLine(response.Success ? "Update Success" : "Update Failed");
            }
            else
            {
                Console.WriteLine("No product to be updated");
            }
        }
        else
        {
            Console.WriteLine("Invalid number or input");
        }
        
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        services.Configure<Options>(configuration.GetSection("ApiSettings"));

        services.AddHttpClient();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IProductService,ProductService>();
    }
}