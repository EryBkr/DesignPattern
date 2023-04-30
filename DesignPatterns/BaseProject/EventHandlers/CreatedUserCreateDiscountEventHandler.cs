using BaseProject.Events;
using BaseProject.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace BaseProject.EventHandlers
{
    //UserCreated bir Notification'du ve onu dinleyecekler class'lardan birisi bu dur
    public class CreatedUserCreateDiscountEventHandler : INotificationHandler<UserCreatedEvent>
    {
        //IServiceProvider'ı eklememe gerek yok, DI da bunu oluşturabiliyorum, Mediatr nimetleri işte
        private readonly ILogger<CreatedUserCreateDiscountEventHandler> _logger;
        private readonly AppIdentityDbContext _context;

        public CreatedUserCreateDiscountEventHandler(ILogger<CreatedUserCreateDiscountEventHandler> logger, AppIdentityDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _context.Discounts.AddAsync(new Discount { Rate = 10, UserId = notification.User.Id });
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Discount was applied for {notification.User.UserName}");
        }
    }
}
