using BaseProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.Repositories
{
    //MSSQL için oluşturduğumuz Repository
    public class ProductRepositorySqlServer : IProductRepository
    {
        private readonly AppIdentityDbContext _context;

        public ProductRepositorySqlServer(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task Delete(Product entity)
        {
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _context.Products.Where(i => i.UserId == userId).ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> Save(Product entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Update(Product entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
