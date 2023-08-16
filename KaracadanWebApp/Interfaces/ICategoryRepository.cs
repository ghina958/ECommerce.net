using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.Categories;

namespace KaracadanWebApp.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories(CategorySearchViewModel? categorySearchViewModel=null);
        Task<Category> GetCategoryById(int id);
        bool Add(Category category );
        bool Update(Category category);
        bool Delete(Category category);
        bool Save();
    }
}
