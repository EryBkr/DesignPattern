using BaseProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace BaseProject.UserCards
{
    //UI tarafında kullanacağımız tag helper
    //<user-card app-user="user"> olarak kullanılacaktır
    public class UserCardTagHelper : TagHelper
    {
        //AppUser UI tarafından parametre olarak verilecek
        public AppUser AppUser { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserCardTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //UI Element burada oluşturuluyor aslında
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            UserCardTemplate userCardTemplate;

            //Giriş yapıldı mı?
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                //Giriş yapıldığıysa kullanıcı içeride daha farklı içerik görecek
                userCardTemplate = new PrimeUserCardTemplate();
            }
            else
            {
                //Üye değilse sınırlı içeriği görecek
                userCardTemplate = new DefaultUserCardTemplate();
            }
            //User'ı Template Class'ına veriyorum
            userCardTemplate.SetUser(AppUser);

            //Abstract class'ımız instance'larına göre HTML Build ediyor
            var htmlContent = userCardTemplate.Build();

            //Oluşturulan HTML set ediliyor
            output.Content.SetHtmlContent(htmlContent);
        }
    }
}
