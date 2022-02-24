using CE.Assessment.BusinessLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        ///Get IN PROGRESS orders
        var orderService = serviceProvider.GetService<IOrderService>();
        var result = orderService.GetInProgressOrders().GetAwaiter().GetResult();

        Console.WriteLine("IN PROGRESS Orders");
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        ///End get IN PROGRESS orders

        ///Get Top 5 Ordered Products
        var top5 = orderService.GetTop5OrderedProducts(result).GetAwaiter().GetResult();
        Console.WriteLine();
        Console.WriteLine("Top 5 Ordered Products");
        for(int i = 0; i < 5; i++)
        {
            if (top5.ElementAtOrDefault(i) is not null)
            {
                Console.WriteLine($"{i + 1}. Product Name = {top5.ElementAt(i).ProductName} || " +
                    $"GTIN = {top5.ElementAt(i).Gtin} || " +
                    $"Qty = {top5.ElementAt(i).TotalQuantity}");
            }
        }
        ///End get top 5 ordered products

        ///Update stock of selected product
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
                var response = productService.UpdateStock(merchatProductNo, 25).GetAwaiter().GetResult();
                Console.WriteLine(response ? "Update Success" : "Update Failed");
                Console.ReadKey();
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
        ///End update stock
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
        services.AddTransient<IProductService, ProductService>();
    }
}