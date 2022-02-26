using CE.Assessment.BusinessLogic.Entities;
using CE.Assessment.BusinessLogic.Helpers;
using CE.Assessment.BusinessLogic.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureServices(IServiceCollection services)
{
    var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

    services.Configure<Options>(configuration.GetSection("ApiSettings"));

    services.AddHttpClient();
    services.AddSingleton<IHttpClientHelper, HttpClientHelper>();
    services.AddTransient<IOrderService, OrderService>();
    services.AddTransient<IProductService, ProductService>();
}