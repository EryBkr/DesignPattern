using BaseProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseProject.Repositories
{
    //Hem Mongo için hem de MSSQL için soyutlama yapacaktır
    public interface IProductRepository
    {
        Task<Product> GetById(string id);
        Task<List<Product>> GetAllByUserId(string userId);
        Task<Product> Save(Product entity);
        Task Update(Product entity);
        Task Delete(Product entity);
    }
}
