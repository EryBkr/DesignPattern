namespace BaseProject.Composite
{
    //Book component kitabı temsil ediyor,agacın en alt noktası altında kategori yok (Composite kavramının zıttı aslında)
    public class BookComponent : IComponent
    {
        public BookComponent(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        //Kitabın altında alt kategori olamaz
        public int Count()
        {
            return 1;
        }

        public string Display()
        {
            return $"<li class='list-group-item'>{Name}</li>";
        }
    }
}
