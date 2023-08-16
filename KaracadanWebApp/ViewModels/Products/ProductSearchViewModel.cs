using Microsoft.AspNetCore.Mvc.Rendering;

namespace KaracadanWebApp.ViewModels.Products
{
    public class ProductSearchViewModel
    {
        public ProductSearchViewModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int ProductsStatus { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> AvailableCategories { get; set; }
    }
}
