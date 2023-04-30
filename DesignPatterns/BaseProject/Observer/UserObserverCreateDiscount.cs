using BaseProject.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace BaseProject.Observer
{
    //Bu arkadaşı Startup'a eklemeyeceğiz ondan dolayı IServiceProvider ile gerekli servisleri handle edeceğim
    public class UserObserverCreateDiscount : IUserObserver
    {
        private readonly IServiceProvider _serviceProvider;

        public UserObserverCreateDiscount(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void UserCreated(AppUser user)
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<UserObserverCreateDiscount>>();

            //Observer Sınıfımı Singleton olarak eklemiştim, Scope farkından dolayı initilaize edememişti,bundan dolayı böyle biz çözüm gerçekleştirdim
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();

            context.Discounts.Add(new Discount { Rate = 10, UserId = user.Id });
            context.SaveChanges();

            logger.LogInformation($"Discount was applied for {user.UserName}");
        }
    }
}
