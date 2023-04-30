using BaseProject.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace BaseProject.Observer
{
    //Bu arkadaşı Startup'a eklemeyeceğiz ondan dolayı IServiceProvider ile gerekli servisleri handle edeceğim
    public class UserObserverWriteToConsole : IUserObserver
    {
        private readonly IServiceProvider _serviceProvider;

        public UserObserverWriteToConsole(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void UserCreated(AppUser user)
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<UserObserverWriteToConsole>>();
            logger.LogInformation($"User Created: {user.Id}");
        }
    }
}
