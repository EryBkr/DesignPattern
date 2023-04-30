using BaseProject.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BaseProject.EventHandlers
{
    //UserCreated bir Notification'du ve onu dinleyecekler class'lardan birisi bu dur
    public class CreatedUserWriteConsoleEventHandler : INotificationHandler<UserCreatedEvent>
    {
        //IServiceProvider'ı eklememe gerek yok, DI da bunu oluşturabiliyorum, Mediatr nimetleri işte
        private readonly ILogger<CreatedUserWriteConsoleEventHandler> _logger;

        public CreatedUserWriteConsoleEventHandler(ILogger<CreatedUserWriteConsoleEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"User Created: {notification.User.Id}");
            return Task.CompletedTask;
        }
    }
}
