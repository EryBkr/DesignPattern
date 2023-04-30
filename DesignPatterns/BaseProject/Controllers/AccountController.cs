using BaseProject.Events;
using BaseProject.Models;
using BaseProject.Observer;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signManager;

        //Gerekli notify(trigger gibi de düşünebilirsiniz) işlemlerini üstlenen sınıfım,Startup'ta gerekli tanımlamalar yapılmıştı zaten
        private readonly UserObserverSubject _userObserverSubject;

        //MediaTr ile event fırlatacağız
        private readonly IMediator _mediator;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signManager, UserObserverSubject userObserverSubject, IMediator mediator)
        {
            _userManager = userManager;
            _signManager = signManager;
            _userObserverSubject = userObserverSubject;
            _mediator = mediator;
        }

        public IActionResult SignUp()
        {
            return View();
        }

        //Kütüphane olmadan kullandığımız Observer yapısı
        //[HttpPost]
        //public async Task<IActionResult> SignUp(CreateUser_VM user)
        //{
        //    var appUser = new AppUser { Email = user.Email, UserName = user.UserName };
        //    var result = await _userManager.CreateAsync(appUser, user.Password);
        //    if (result.Succeeded)
        //    {
        //        ViewBag.Message = "Üyelik işlemi başarıyla gerçekleştirildi";

        //        //Buraya Mail atma, indirim yapma gibi metotları ekleyebilirdik fakat bu şekilde daha clean ve bağımsız bir kod yazdık
        //        _userObserverSubject.NotifyObservers(appUser);
        //    }
        //    else
        //    {
        //        ViewBag.Message = result.Errors.ToList().First().Description;
        //        return View();
        //    }


        //    return RedirectToAction(nameof(HomeController.Index), "Home");
        //}


        //MediaTR ile kullandığımız Observer yapısı
        [HttpPost]
        public async Task<IActionResult> SignUp(CreateUser_VM user)
        {
            var appUser = new AppUser { Email = user.Email, UserName = user.UserName };
            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded)
            {
                ViewBag.Message = "Üyelik işlemi başarıyla gerçekleştirildi";

                //Buraya Mail atma, indirim yapma gibi metotları ekleyebilirdik fakat bu şekilde daha clean ve bağımsız bir kod yazdık
                //Publish Event fırlatır ve ilgili handler'lar içeriği yakalar,ben havaya atarım kim yakalarsa ;)
                await _mediator.Publish(new UserCreatedEvent { User = appUser });
            }
            else
            {
                ViewBag.Message = result.Errors.ToList().First().Description;
                return View();
            }


            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hasUser = await _userManager.FindByEmailAsync(email);
            if (hasUser == null) return View();

            var signInResult = await _signManager.PasswordSignInAsync(hasUser, password, true, false);

            if (!signInResult.Succeeded)
                return View();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
