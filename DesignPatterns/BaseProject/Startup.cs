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

            //Giriþ yapmýþ kullanýcýyý alabilmek için
            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
            services.AddDbContext<AppIdentityDbContext>(options => { options.UseSqlServer(Configuration.GetConnectionString("SqlServer")); });
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            //Artýk IProductRepository cache decorator (ya da log decorator) dönüyor ve constructor da gereken parametreleri biz temin ediyoruz
            //KÜTÜPHANE YOKKEN BU ÞEKÝLDE DI TARAFINI COZEBÝLÝRDÝK
            //services.AddScoped<IProductRepository>(sp =>
            //{
            //    var dbContext = sp.GetRequiredService<AppIdentityDbContext>();
            //    var memoryCache = sp.GetRequiredService<IMemoryCache>();
            //    var productRepository = new ProductRepository(dbContext);
            //    var logProvider = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();

            //    var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);

            //    //Cache decorator üzerinden çalýþmasýný istediðim için onu geçtim,direkt appidentitydbcontext te verebilirdim ama cache özellikleri olmazdý
            //    var logDecorator = new ProductRepositoryLoggingDecorator(cacheDecorator, logProvider);

            //    return logDecorator;
            //});

            //Decorator için DI çözümünü Scrutor Nuget paketi ile daha sade çözebiliriz
            //services.
            //    AddScoped<IProductRepository, ProductRepository>().
            //    Decorate<IProductRepository, ProductRepositoryCacheDecorator>().
            //    Decorate<IProductRepository, ProductRepositoryLoggingDecorator>();

            //Run Time'da hangi Decorator'un yüklenmesini istiyorsam þayet bu þekilde resolve edebilirim
            services.AddScoped<IProductRepository>(sp =>
            {
                //Login olmuþ kullanýcý bilgisini almak için ekledim
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var dbContext = sp.GetRequiredService<AppIdentityDbContext>();
                var memoryCache = sp.GetRequiredService<IMemoryCache>();
                var productRepository = new ProductRepository(dbContext);
                var logProvider = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();

                //Giriþ Yapan kullanýcýya göre instance alýnacak
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
