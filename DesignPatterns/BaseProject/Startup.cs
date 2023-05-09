using BaseProject.Models;
using BaseProject.Repositories;
using BaseProject.Repositories.Decorator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            //Giri� yapm�� kullan�c�y� alabilmek i�in
            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
            services.AddDbContext<AppIdentityDbContext>(options => { options.UseSqlServer(Configuration.GetConnectionString("SqlServer")); });
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            //Art�k IProductRepository cache decorator (ya da log decorator) d�n�yor ve constructor da gereken parametreleri biz temin ediyoruz
            //K�T�PHANE YOKKEN BU �EK�LDE DI TARAFINI COZEB�L�RD�K
            //services.AddScoped<IProductRepository>(sp =>
            //{
            //    var dbContext = sp.GetRequiredService<AppIdentityDbContext>();
            //    var memoryCache = sp.GetRequiredService<IMemoryCache>();
            //    var productRepository = new ProductRepository(dbContext);
            //    var logProvider = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();

            //    var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);

            //    //Cache decorator �zerinden �al��mas�n� istedi�im i�in onu ge�tim,direkt appidentitydbcontext te verebilirdim ama cache �zellikleri olmazd�
            //    var logDecorator = new ProductRepositoryLoggingDecorator(cacheDecorator, logProvider);

            //    return logDecorator;
            //});

            //Decorator i�in DI ��z�m�n� Scrutor Nuget paketi ile daha sade ��zebiliriz
            //services.
            //    AddScoped<IProductRepository, ProductRepository>().
            //    Decorate<IProductRepository, ProductRepositoryCacheDecorator>().
            //    Decorate<IProductRepository, ProductRepositoryLoggingDecorator>();

            //Run Time'da hangi Decorator'un y�klenmesini istiyorsam �ayet bu �ekilde resolve edebilirim
            services.AddScoped<IProductRepository>(sp =>
            {
                //Login olmu� kullan�c� bilgisini almak i�in ekledim
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var dbContext = sp.GetRequiredService<AppIdentityDbContext>();
                var memoryCache = sp.GetRequiredService<IMemoryCache>();
                var productRepository = new ProductRepository(dbContext);
                var logProvider = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();

                //Giri� Yapan kullan�c�ya g�re instance al�nacak
                if (httpContextAccessor.HttpContext.User.Identity.Name == "Eray")
                {
                    var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);
                    return cacheDecorator;
                }

                var logDecorator = new ProductRepositoryLoggingDecorator(productRepository, logProvider);
                return logDecorator;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
