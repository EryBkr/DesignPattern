namespace BaseProject.UserCards
{
    //Bu arkadaş arayüzde sınırlı içerik gösterecektir
    public class DefaultUserCardTemplate : UserCardTemplate
    {
        //Bu Card'ın Footer'ı yok
        protected override string SetFooter()
        { return string.Empty; }

        protected override string SetPicture()
        {
            //Resmi Card üzerinde oluşturmuş olduk
            return $"<img src ='/pictures/userdefault.png' class='card-img-top'>";
        }
    }
}
