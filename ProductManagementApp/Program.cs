using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Data;
using ProductManagementApp.Services;
using ProductManagementApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

// Configure database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)); // UseSqlServer for SQL Server

// Configure external API settings
builder.Services.Configure<ExternalApiSettings>(builder.Configuration.GetSection("ExternalApiSettings"));

// Configure HttpClient services
builder.Services.AddHttpClient<IExternalProductService, ExternalProductService>(client =>
{
    var apiSettings = builder.Configuration.GetSection("ExternalApiSettings").Get<ExternalApiSettings>();
    client.BaseAddress = new Uri(apiSettings.BaseAddress);
});

builder.Services.AddHttpClient<IProductInfoService, ProductInfoService>(client =>
{
    client.BaseAddress = new Uri("https://fakestoreapi.com/"); // Adjust if needed
});

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
