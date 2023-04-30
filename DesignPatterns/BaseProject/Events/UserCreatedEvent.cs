using BaseProject.Models;
using MediatR;

namespace BaseProject.Events
{
    //Mediatr uygulaması
    //Observer Pattern'i burada ki örnekte Mediatr yardımıyla uygulayacağız
    public class UserCreatedEvent : INotification
    {
        public AppUser User { get; set; }
    }
}
