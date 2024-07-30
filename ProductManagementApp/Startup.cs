using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductManagementApp.Data;
using ProductManagementApp.Services;
using ProductManagementApp.Models;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
        services.Configure<ExternalApiSettings>(Configuration.GetSection("ExternalApiSettings"));


        services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        
        services.AddControllersWithViews();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        // Register the external API service
        services.AddHttpClient<IExternalProductService, ExternalProductService>(client =>
        {
            var apiSettings = Configuration.GetSection("ExternalApiSettings").Get<ExternalApiSettings>();
            client.BaseAddress = new Uri(apiSettings.BaseAddress);
        });

        // Register the external API service
        services.AddHttpClient<IExternalProductService, ExternalProductService>();
        // Register HttpClient and IProductInfoService
        // Register HttpClient and IProductInfoService
        services.AddHttpClient<IProductInfoService, ProductInfoService>(client =>
        {
            var apiSettings = Configuration.GetSection("ExternalApiSettings").Get<ExternalApiSettings>();
            client.BaseAddress = new Uri(apiSettings.BaseAddress);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapDefaultControllerRoute();

        });
    }
}
