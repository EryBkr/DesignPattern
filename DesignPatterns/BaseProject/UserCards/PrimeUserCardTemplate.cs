using System.Text;

namespace BaseProject.UserCards
{
    //Bu arkadaş arayüzde Full içerik gösterecektir

    public class PrimeUserCardTemplate : UserCardTemplate
    {
        //Üye olmuş olan kullanıcının Footer'ı dolu olacaktır
        protected override string SetFooter()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<a href='#' class='btn btn-primary'>Mesaj Gönder</a>");
            stringBuilder.Append("<a href='#' class='btn btn-primary'>Profili Gör</a>");

            return stringBuilder.ToString();
        }

        protected override string SetPicture()
        {
            //Resmi Card üzerinde oluşturmuş olduk
            return $"<img src ='/pictures/beyblade.jpg' class='card-img-top'>";
        }
    }
}
