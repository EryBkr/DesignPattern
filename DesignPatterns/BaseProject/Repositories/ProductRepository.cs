using BaseProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.Repositories
{
    //Standart bir repository fakat Decorator design pattern aslında ara bir manager service gibi çalışacak ve buradaki işlerin başına ya da sonuna bazı işlemler yapmamıza olanak sağlayacaktır
    public class ProductRepository : IProductRepository
    {
        private readonly AppIdentityDbContext _context;

        public ProductRepository(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> GetAll(string userId)
        {
            return await _context.Products
                .Where(i => i.UserId == userId)
                .ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task Remove(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> Save(Product product)
        {
            var entity = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
