using BaseProject.Models;
using BaseProject.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddControllersWithViews();
            services.AddDbContext<AppIdentityDbContext>(options => { options.UseSqlServer(Configuration.GetConnectionString("SqlServer")); });
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            //Cookie'e eri�ebilmek i�in ekledik
            services.AddHttpContextAccessor();

            //Strategy Pattern'i burada uyguluyoruz, yap�lan se�ime g�re instance �retip veriyoruz (in Runtime)
            services.AddScoped<IProductRepository>(sp =>
            {
                //Cookie i�in kullanaca��z
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                //Giri� yapm�� kullanc�n�n Database Settings'ini bar�nd�ran Claim'ine eri�iyorum
                var claim = httpContextAccessor.HttpContext.User.Claims.Where(i => i.Type == Settings.ClaimDatabaseType).FirstOrDefault();

                //DbContext'i ald�k
                var context = sp.GetRequiredService<AppIdentityDbContext>();

                //Claim bo� ise default olarak MsSQL'e ba�lans�n
                if (claim == null) return new ProductRepositorySqlServer(context);

                //Claim den se�ilen db ayar�n� al�yorum
                var databaseType = (DatabaseTypeEnum)int.Parse(claim.Value);

                //Ki�inin se�imine g�re db'nin instance'n� alaca��z
                return databaseType switch
                {
                    DatabaseTypeEnum.SqlServer => new ProductRepositorySqlServer(context),
                    DatabaseTypeEnum.MongoDB => new ProductRepositoryMongoDb(Configuration),
                    _ => throw new NotImplementedException()
                };
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
