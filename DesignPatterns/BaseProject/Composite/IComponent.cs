namespace BaseProject.Composite
{
    //Composite Design Pattern genelde sonsuz kategori algoritmalarında kullanılır
    public interface IComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Sub Items
        int Count();

        //html elemanını tutacak
        string Display();
    }
}
