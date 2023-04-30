using BaseProject.Models;

namespace BaseProject.Observer
{
    //Observer Pattern'in amacı bir işlem yapıldıktan sonra onu takiben yapılacak diğer işlemleri notify etmektir, işlemin yapıldığı metota alt alta metotları çağırmaktan daha soyut bir çözüm sağlamaktadır
    public interface IUserObserver
    {
        void UserCreated(AppUser user);
    }
}
