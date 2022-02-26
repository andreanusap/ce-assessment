using CE.Assessment.BusinessLogic.Helpers;
using CE.Assessment.BusinessLogic.Services;
using CE.Assessment.Shared.Entities;
using CE.Assessment.WebApi.Middlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;

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

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

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

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "CE Assessmet Web API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"Enter the api key in the text input below.Example: '12345abcdef'",
            Name = "XApiKey",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                },
                new List<string>()
            }
        });
    });
}