using BaseProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                var user1 = new AppUser() { UserName = "Eray", Email = "crazyeray94@gmail.com" };
                var user2 = new AppUser() { UserName = "Berkay", Email = "crazyberkay00@gmail.com" };
                var user3 = new AppUser() { UserName = "User1", Email = "crazyuser1@gmail.com" };

                userManager.CreateAsync(user1, "AllStar94*").Wait();
                userManager.CreateAsync(user2, "AllStar94*").Wait();
                userManager.CreateAsync(user3, "AllStar94*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "User2", Email = "crazuser2@gmail.com" }, "AllStar94*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "User3", Email = "crazyuser3@gmail.com" }, "AllStar94*").Wait();

                //Main Categories
                var sucRomanlari = identityDbContext.Categories.Add(new Category { Name = "Suç Romanlarý", ReferenceId = 0, UserId = user1.Id });
                var fantastic = identityDbContext.Categories.Add(new Category { Name = "Fantastik Evren Romanlarý", ReferenceId = 0, UserId = user2.Id });
                var policy = identityDbContext.Categories.Add(new Category { Name = "Polisiye Romanlarý", ReferenceId = 0, UserId = user3.Id });
                identityDbContext.SaveChanges();

                //Sub Categories
                var englishCriminal = identityDbContext.Categories.Add(new Category { Name = "Ýngiliz Suç Romanlarý", ReferenceId = sucRomanlari.Entity.Id, UserId = user1.Id });
                var lotrSeries = identityDbContext.Categories.Add(new Category { Name = "Lord of The Rings Series", ReferenceId = fantastic.Entity.Id, UserId = user1.Id });
                var sherlockSeries = identityDbContext.Categories.Add(new Category { Name = "Sherlock Holmes Series", ReferenceId = policy.Entity.Id, UserId = user1.Id });
                identityDbContext.SaveChanges();

                //Sub-sub categories
                var englishCriminalSub = identityDbContext.Categories.Add(new Category { Name = "1984 Öncesi Ýngiliz Suç Romanlarý", ReferenceId = englishCriminal.Entity.Id, UserId = user1.Id });
                var lotrSeriesNew = identityDbContext.Categories.Add(new Category { Name = "Lord of The Rings 2010 Sonrasý", ReferenceId = lotrSeries.Entity.Id, UserId = user1.Id });
                var sherlockSub = identityDbContext.Categories.Add(new Category { Name = "Sherlock Holmes vs Agatha Cristine Series", ReferenceId = sherlockSeries.Entity.Id, UserId = user1.Id });
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
