using CE.Assessment.Web.Helpers;
using CE.Assessment.Web.Models;
using CE.Assessment.Web.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void ConfigureServices(IServiceCollection services)  {
    var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

    services.Configure<ApiOptions>(configuration.GetSection("ApiSettings"));

    services.AddAutoMapper(typeof(MappingProfile));

    services.AddHttpClient<IHomeService, HomeService>((sp, client) =>
    {
        var apiOptions = sp.GetRequiredService<IOptions<ApiOptions>>().Value;
        client.BaseAddress = new Uri(apiOptions.BaseUrl);
        client.DefaultRequestHeaders.Add("XApiKey", apiOptions.XApiKey);
    });
}