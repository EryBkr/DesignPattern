using System.Collections.Generic;

namespace BaseProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public string UserId { get; set; }
        public ICollection<Book> Books { get; set; }

        //Sonsuz kategori mantığı aslında,bu kategori hangi kategoriye bağlı? ReferenceId==0 ise main kategori olmuş oluyor
        public int ReferenceId { get; set; }
    }
}
