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

        #region "IN PROGRESS"
        var orderService = serviceProvider.GetService<IOrderService>();
        var result = orderService.GetInProgressOrders().GetAwaiter().GetResult();

        Console.WriteLine("IN PROGRESS Orders");
        Console.WriteLine();

        var table = new ConsoleTables.ConsoleTable("Channel Order No", "Merchant Order No", "Payment Method", "Order Date", "Sub Total Vat",
            "Sub Total Incl Vat", "Shipping Cost Vat", "Shipping Cost Incl Vat", "Total Vat", "Total Incl Vat");
        
        result.ToList()
            .ForEach(x => table.AddRow(x.ChannelOrderNo, x.MerchantOrderNo, x.PaymentMethod, x.OrderDate, x.SubTotalVat, x.SubTotalInclVat,
            x.ShippingCostsVat, x.ShippingCostsInclVat, x.TotalVat, x.TotalInclVat));

        table.Write();
        Console.WriteLine();
        #endregion

        #region "Top 5 Products"
        var top5 = orderService.GetTop5OrderedProducts(result).GetAwaiter().GetResult();
        Console.WriteLine("Top 5 Ordered Products");
        Console.WriteLine();

        var table2 = new ConsoleTables.ConsoleTable("No.", "Merchant Product No", "Product Name", "Gtin", "Total Quantity");

        var top5List = top5
            .Select((value, index) => new { index, value.MerchantProductNo, value.ProductName, value.Gtin, value.TotalQuantity })
            .ToList();

        top5List.ForEach(x => table2.AddRow(x.index + 1, x.MerchantProductNo, x.ProductName, x.Gtin, x.TotalQuantity));

        table2.Write();
        Console.WriteLine();
        #endregion

        #region "Update"
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
        #endregion
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