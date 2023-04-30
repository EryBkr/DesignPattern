using BaseProject.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BaseProject.EventHandlers
{
    //UserCreated bir Notification'du ve onu dinleyecekler class'lardan birisi bu dur
    public class CreatedUserSendEmailEventHandler : INotificationHandler<UserCreatedEvent>
    {
        //IServiceProvider'ı eklememe gerek yok, DI da bunu oluşturabiliyorum, Mediatr nimetleri işte
        private readonly ILogger<CreatedUserSendEmailEventHandler> _logger;

        public CreatedUserSendEmailEventHandler(ILogger<CreatedUserSendEmailEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            //Send Mail
            _logger.LogInformation($"Send Email to {notification.User.Email}");

            return Task.CompletedTask;
        }
    }
}
