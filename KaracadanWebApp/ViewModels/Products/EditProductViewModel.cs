using KaracadanWebApp.Data;
using KaracadanWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KaracadanWebApp.ViewModels.Products
{
    public class EditProductViewModel
    {
        public EditProductViewModel()
        {
            Categories = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? URL { get; set; }
        public int Price { get; set; }
        public ProductsStatus ProductsStatus { get; set; }

        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; }

    }
}
