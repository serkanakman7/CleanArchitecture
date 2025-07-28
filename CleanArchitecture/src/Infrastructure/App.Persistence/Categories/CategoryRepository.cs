using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Categories
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

        public Task<List<Category>> GetCategoryWithProductsAsync()
        {
            return Context.Categories.Include(x => x.Products).AsQueryable().ToListAsync();
        }
    }
}
