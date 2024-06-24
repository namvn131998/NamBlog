using ShoppingCart.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Business.Repositories;
using NuGet.Protocol.Core.Types;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(serviceProvider => new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=namvnstorage;AccountKey=3LjRglhRy1x16i7RxYhu1yJ1Ty0NRilp3f6Vyb2/UDRNivdQo/UkoxLBgEdstxD6Nk15HPwiB6+5+AStJhh7hA==;EndpointSuffix=core.windows.net"));
builder.Services.AddScoped<IBlobRepository, BlobRepository>();

builder.Services.AddDistributedMemoryCache();
//Add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);//Session Timeout.
    options.Cookie.Name = ".ShoppingCart.Session";
});

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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}");

app.Run();
