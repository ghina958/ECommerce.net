using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;

namespace KaracadanWebApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategories(CategorySearchViewModel? categorySearchViewModel = null)
        {
            if (categorySearchViewModel == null)
            {
                return await _context.Categories.ToListAsync();
            }
            var result = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(categorySearchViewModel.Name))
            {
                result = result.Where(x => x.Name == categorySearchViewModel.Name);
            }

            return await result.ToListAsync();
        }

            public async Task<Category?> GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(i => i.Id == id);
            //_context.Categories.
        }


        public bool Add(Category category)
        {
            _context.Categories.Add(category);
            return Save();
        }

        public bool Delete(Category category)
        {
            _context.Categories.Remove(category);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Category category)
        {
            _context.Categories.Update(category);
            return Save();

        }
    }
}
