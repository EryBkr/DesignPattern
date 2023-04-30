using BaseProject.Models;
using System.Collections.Generic;

namespace BaseProject.Observer
{
    //Observer Patterne göre bir işlem yapıldıktan sonra onu takiben tetiklenecek işlemler vardır,bunları açık açık ilk işlemin yapıldığı yere yazmak loosecoupling'e aykırıdır
    //Bundan dolayı yapılacak işlemler listesine ekleme yapıyorum ve finalde notify metodu ile işlemler execute edilmiş oluyor
    public class UserObserverSubject
    {
        private readonly List<IUserObserver> _userObservers;

        public UserObserverSubject()
        {
            _userObservers = new List<IUserObserver>();
        }

        public void RegisterObserver(IUserObserver userObserver)
        {
            _userObservers.Add(userObserver);
        }

        public void UndoObserver(IUserObserver userObserver)
        {
            _userObservers.Remove(userObserver);
        }

        public void NotifyObservers(AppUser user)
        {
            _userObservers.ForEach(observer =>
            {
                observer.UserCreated(user);
            });
        }
    }
}
