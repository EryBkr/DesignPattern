using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseProject.Models
{
    //Hem MongoDB hem de SQL Db için kullanacağım modelim olur kendileri
    public class Product
    {
        //MongoDB kendisi unique bir değer atıyor,MSSQL için de biz Guid bir değer atacağız
        [Key] //EF Core için ekledik, şart değildi
        [BsonId] //MongoDB nin bu property'nin ID olduğunu bilmesi için ekledik
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] //ID nin string türü olduğunu belirledik
        public string Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")] //EF Core için ekledik
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)] //Bu arkadaşın decimal olduğunu belirledik
        public decimal Price { get; set; }

        public int Stock { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)] //Bu arkadaşın DateTime olduğunu belirledik
        public DateTime CreatedDate { get; set; }

        //İlişki için ekledik
        public string UserId { get; set; }
    }
}
