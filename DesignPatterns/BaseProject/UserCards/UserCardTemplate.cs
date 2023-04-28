using BaseProject.Models;
using System;
using System.Text;

namespace BaseProject.UserCards
{
    //Template pattern aslında işlevlerin merkezileştirilmesiyle ilgilidir. İlgili bölümde ki ortak kısımlar base'de toplanır ve özelliştirmeler inheritance olarak çözümlenir
    public abstract class UserCardTemplate
    {
        protected AppUser appUser { get; set; }

        public void SetUser(AppUser appUser)
        {
            this.appUser = appUser;
        }

        //Çalışma sırası ve işlevlerin toplantığı yer,sonuçta bir şey oluşturuyorum
        //SetPicture ve SetFooter kullanıcının üye olup olmadığı senaryolara göre farklılık gösterebilir,onların abstract olma nedenleri budur
        public string Build()
        {
            if (appUser == null) throw new ArgumentNullException(nameof(AppUser));

            var stringBuilder = new StringBuilder();

            //Bootstrap Card oluşturuyoruz
            stringBuilder.Append("<div class='card' style='width: 18rem;'>");

                //Set Picture durum ve şartlara göre değişeceği için bu şekilde uyguladık
                stringBuilder.Append(SetPicture());

                stringBuilder.Append($@"
                <div class='card-body>'
                    <h5>{appUser.UserName}</h5>
                    <p>{appUser.Description}</p>
                ");
                //Varsa eğer Footer'daki itemlar gösterilecek
                stringBuilder.Append(SetFooter());
                stringBuilder.Append("</div>");

            stringBuilder.Append("</div>");

            return stringBuilder.ToString();
        }

        //User Card'ın Footer ve Picture işlevleri duruma göre değişir
        protected abstract string SetFooter();
        protected abstract string SetPicture();
    }
}
