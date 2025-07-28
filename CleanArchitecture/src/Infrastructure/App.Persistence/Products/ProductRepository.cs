using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using App.Persistence;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Products
{
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetTopPriceProductsAsync(int count)
        {
            return await Context.Products.OrderByDescending(x => x.Price).Take(count).ToListAsync();
        }
    }
}
