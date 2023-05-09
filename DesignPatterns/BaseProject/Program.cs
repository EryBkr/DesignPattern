using BaseProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace BaseProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            identityDbContext.Database.Migrate();

            if (!userManager.Users.Any())
            {
                var user = new AppUser() { UserName = "Eray", Email = "crazyeray94@gmail.com" };
                userManager.CreateAsync(user, "AllStar94*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "Berkay", Email = "crazyberkay00@gmail.com" }, "AllStar94*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "User1", Email = "crazyuser1@gmail.com" }, "AllStar94*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "User2", Email = "crazuser2@gmail.com" }, "AllStar94*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "User3", Email = "crazyuser3@gmail.com" }, "AllStar94*").Wait();

                Enumerable.Range(1, 100).ToList().ForEach(product =>
                {
                    identityDbContext.Products.Add(new Product { Name = $"Product {product}", Price = product * 13, Stock = product * 7, UserId = user.Id });
                });

                identityDbContext.SaveChanges();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
