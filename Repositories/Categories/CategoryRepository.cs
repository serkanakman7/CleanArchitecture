using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories.Categories
{
    public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            return Context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Category> GetCategoryWithProducts()
        {
            return Context.Categories.Include(x => x.Products).AsQueryable();
        }
    }
}
