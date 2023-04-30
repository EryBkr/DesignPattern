using BaseProject.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace BaseProject.Observer
{
    //Bu arkadaşı Startup'a eklemeyeceğiz ondan dolayı IServiceProvider ile gerekli servisleri handle edeceğim
    public class UserObserverSendEmail : IUserObserver
    {
        private readonly IServiceProvider _serviceProvider;

        public UserObserverSendEmail(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void UserCreated(AppUser user)
        {
            //Send Mail
            var logger = _serviceProvider.GetRequiredService<ILogger<UserObserverSendEmail>>();
            logger.LogInformation($"Send Email to {user.Email}");
        }
    }
}
