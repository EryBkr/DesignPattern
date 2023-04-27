using BaseProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BaseProject.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SettingsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            Settings settings = new Settings();

            //Giriş yapmış kullancının Database Ayarı ile ilgili bilgisini Cookie'de olan Claim üzerinden al
            var logInUserDatabaseClaim = User.Claims.Where(i => i.Type == Settings.ClaimDatabaseType).FirstOrDefault();

            if (logInUserDatabaseClaim != null)
                settings.DatabaseType = (DatabaseTypeEnum)int.Parse(logInUserDatabaseClaim.Value);
            else
                settings.DatabaseType = settings.DefaultDatabase;


            return View(settings);
        }

        //Kullanıcının seçmiş olduğu database'i Claim'ine atıyoruz,artık o kullanıcının yapacağı CRUD işlemleri seçmiş olduğu DB üzerinden gerçekleştirilecek
        [HttpPost]
        public async Task<IActionResult> ChangeDatabase(int databaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            //Kullanıcının seçmiş olduğu db ayarı Claim'ine kaydedilecek,Claim'i oluşturuyoruz
            var newClaim = new Claim(Settings.ClaimDatabaseType, databaseType.ToString());

            var claims = await _userManager.GetClaimsAsync(user);

            var hasDatabaseTypeClaim = claims.FirstOrDefault(i => i.Type == Settings.ClaimDatabaseType);

            //Database ayarı kullanıcıya atanmış ise yenisiyle değiştiriyoruz
            //Yoksa ekliyoruz
            if (hasDatabaseTypeClaim != null)
                await _userManager.ReplaceClaimAsync(user, hasDatabaseTypeClaim, newClaim);
            else
                await _userManager.AddClaimAsync(user, newClaim);

            //Kullanıcının Cookie'sinde eski Claim kalmış olabilir, o yüzden gir çık yapıyoruz
            await _signInManager.SignOutAsync();

            //Kullanıcı beni hatırla vs... demiş olabilir,o ayarları handle ediyoruz ki tekrar giriş yaptırdığımızda kullanıcıyı ayarları değişmiş olmasın
            var authResult = await HttpContext.AuthenticateAsync();

            await _signInManager.SignInAsync(user, authResult.Properties);

            return RedirectToAction(nameof(Index));
        }
    }
}
