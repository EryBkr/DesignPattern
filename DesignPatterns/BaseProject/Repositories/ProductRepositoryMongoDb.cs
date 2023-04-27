using BaseProject.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseProject.Repositories
{
    //MongoDB içi çalışacak Repository Class
    public class ProductRepositoryMongoDb : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepositoryMongoDb(IConfiguration configuration)
        {
            var mongoDbConnString = configuration.GetConnectionString("MongoDb");
            var client = new MongoClient(mongoDbConnString);

            //Yok ise database'i oluşturacak Mongo'nun böyle bir tatlılığı var
            var database = client.GetDatabase("ProductDb");

            //Tabloyu aldık
            _productCollection = database.GetCollection<Product>("Products");
        }

        public async Task Delete(Product entity)
        {
            await _productCollection.DeleteOneAsync(i => i.Id == entity.Id);
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _productCollection.Find(i => i.UserId == userId).ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            return await _productCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> Save(Product entity)
        {
            await _productCollection.InsertOneAsync(entity);
            return entity;
        }

        public async Task Update(Product entity)
        {
            await _productCollection.FindOneAndReplaceAsync(i => i.Id == entity.Id, entity);
        }
    }
}
