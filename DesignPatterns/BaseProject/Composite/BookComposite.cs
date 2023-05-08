using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseProject.Composite
{
    //Composite kavramı aslında sub itemlara sahip olanları nitelendiriyor
    public class BookComposite : IComponent
    {
        public BookComposite(int id, string name)
        {
            Id = id;
            Name = name;
            _components = new List<IComponent>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        private List<IComponent> _components;

        //Dış dünyaya readonly olarak açıyoruz
        public IReadOnlyCollection<IComponent> Components => _components;

        //Komponent listesine ekleme yapacak
        public void Add(IComponent component)
        {
            _components.Add(component);
        }

        //Komponent listesinden silme işlemi yapacak
        public void Remove(IComponent component)
        {
            _components.Remove(component);
        }

        //Altında kalan bütün eleman adetlerini toplayacak
        public int Count()
        {
            return _components.Sum(i => i.Count());
        }

        //Sonsuz kategorinin html çıktısı
        public string Display()
        {
            var sb = new StringBuilder();
            sb.Append($"<div class='text-primary my-1'><a href='#' class='menu'>{Name}</a> ({Count()})</div>");

            //Altında başka bir kategori yoksa bu haliyle menüyü dönebiliriz
            if (!_components.Any()) return sb.ToString();

            sb.Append($"<ul class='list-group list-group-flush ml-3'>");

            foreach (var item in _components)
            {
                //Item'ın display metodundan dönen string'i ekliyorum,Component ise aslında li ekleyecek değilse recursive gibi çalışıp menüyü eklemeye devam edecek
                sb.Append(item.Display());
            }

            sb.Append("</ul>");

            return sb.ToString();
        }

        //Drowdown oluşturuyorum (Sadece composite'lerden oluşacak)
        public List<SelectListItem> GetSelectListItems(string line)
        {
            //bir adet kategori otomatik olarak eklendi
            var list = new List<SelectListItem> { new SelectListItem($"{line}{Name}", Id.ToString()) };

            //componenlerin içerisinde BookComposite varsa (yani cast edilebiliyorsa)
            if (_components.Any(i => i is BookComposite))
            {
                line += " - ";
            }

            _components.ForEach(i =>
            {
                if (i is BookComposite bookComposite)
                {
                    list.AddRange(bookComposite.GetSelectListItems(line));
                }
            });

            return list;
        }
    }
}
