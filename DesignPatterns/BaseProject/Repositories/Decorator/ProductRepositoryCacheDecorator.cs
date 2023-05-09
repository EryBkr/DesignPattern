using BaseProject.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.Repositories.Decorator
{
    //Base Decorator'müz hazırdı zaten,özelleşmiş işlemler için class'lar ekleyeceğim, bu da o class'lardan birisi
    public class ProductRepositoryCacheDecorator : BaseProductRepositoryDecorator
    {
        //In Memory Cache kullanıyoruz
        private readonly IMemoryCache _memoryCache;
        private const string ProductsCacheName = "products";


        public ProductRepositoryCacheDecorator(IProductRepository productRepository, IMemoryCache memoryCache) : base(productRepository)
        {
            _memoryCache = memoryCache;
        }

        public async override Task<List<Product>> GetAll()
        {
            //Cache datası mevcut mu? Mevcut ise onu dönelim
            if (_memoryCache.TryGetValue<List<Product>>(ProductsCacheName, out List<Product> cacheProducts)) return cacheProducts;

            //Cache'e ekleme yapıyoruz (ya da güncelleme)
            await UpdateCache();

            //Her türlü cache'ten data dönüyoruz
            return _memoryCache.Get<List<Product>>(ProductsCacheName);
        }

        public async override Task<List<Product>> GetAll(string userId)
        {
            //GetAll Cache'ten çalışıyor
            var products = await GetAll();
            return products.Where(i => i.UserId == userId).ToList();
        }

        public async override Task<Product> Save(Product product)
        {
            await base.Save(product);
            //Ekleme işlemi yapıldığı için Cache'i refreshliyoruz
            await UpdateCache();

            return product;
        }

        public async override Task Update(Product product)
        {
            await base.Update(product);

            //Güncelleme işlemi yapıldığı için Cache'i refreshliyoruz
            await UpdateCache();
        }

        public async override Task Remove(Product product)
        {
            await base.Remove(product);

            //Silme işlemi yapıldığı için Cache'i refreshliyoruz
            await UpdateCache();
        }

        //Cache Güncelleme işlemi
        private async Task UpdateCache()
        {
            _memoryCache.Set(ProductsCacheName, await base.GetAll());
        }
    }
}
