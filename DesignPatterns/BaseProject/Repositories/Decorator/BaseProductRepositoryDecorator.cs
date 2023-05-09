using BaseProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseProject.Repositories.Decorator
{
    //Decorator mevcutta çalışan bir serviste değişiklik yapmadan onu genişleterek araya işlem eklememizi clean bir şekilde sağlayan pattern'dir
    public class BaseProductRepositoryDecorator : IProductRepository
    {
        public readonly IProductRepository _productRepository;

        public BaseProductRepositoryDecorator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //Ezmemiz gerekirse diye virtual tanımladık
        public virtual async Task<List<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public virtual async Task<List<Product>> GetAll(string userId)
        {
            return await _productRepository.GetAll(userId);
        }

        public virtual async Task<Product> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public virtual async Task Remove(Product product)
        {
            await _productRepository.Remove(product);
        }

        public virtual async Task<Product> Save(Product product)
        {
            return await _productRepository.Save(product);
        }

        public virtual async Task Update(Product product)
        {
            await _productRepository.Update(product);
        }
    }
}
