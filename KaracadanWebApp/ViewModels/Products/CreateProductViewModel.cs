using KaracadanWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace KaracadanWebApp.ViewModels.Products
{
    public class CreateProductViewModel
    {
        public CreateProductViewModel()
        {
            Categories = new List<SelectListItem>();
        }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile Image { get; set; }
        public int Price { get; set; }
        public ProductsStatus ProductsStatus { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
