using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.Products;

namespace KaracadanWebApp.ViewModels.Categories
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {
            CategorySearchViewModel = new CategorySearchViewModel();
        }
        public List<Category>? Categories { get; set; }
        public CategorySearchViewModel? CategorySearchViewModel { get; set; }
    }
}
