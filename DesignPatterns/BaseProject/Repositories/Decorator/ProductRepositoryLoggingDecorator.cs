using BaseProject.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseProject.Repositories.Decorator
{
    //Bu arkadaşta Log işlemi için ProductRepository işlemlerinin arasına girerek onu genişleyecektir, Cache mi Log işlemi mi yapılacağına biz run timeda da karar verebiliriz
    public class ProductRepositoryLoggingDecorator : BaseProductRepositoryDecorator
    {
        private readonly ILogger<ProductRepositoryLoggingDecorator> _logger;
        public ProductRepositoryLoggingDecorator(IProductRepository productRepository, ILogger<ProductRepositoryLoggingDecorator> logger) : base(productRepository)
        {
            _logger = logger;
        }

        public override Task<List<Product>> GetAll()
        {

            _logger.LogInformation("GetAll metodu çalıştı");
            return base.GetAll();
        }

        public override Task<List<Product>> GetAll(string userId)
        {
            _logger.LogInformation("GetAll(UserId) metodu çalıştı");
            return base.GetAll(userId);
        }

        public override Task<Product> Save(Product product)
        {
            _logger.LogInformation("Save metodu çalıştı");
            return base.Save(product);
        }

        public override Task<Product> GetById(int id)
        {
            _logger.LogInformation($"GetById({id}) metodu çalıştı");
            return base.GetById(id);
        }

        public override Task Remove(Product product)
        {
            _logger.LogInformation("Remove metodu çalıştı");
            return base.Remove(product);
        }

        public override Task Update(Product product)
        {
            _logger.LogInformation("Update metodu çalıştı");
            return base.Update(product);
        }
    }
}
