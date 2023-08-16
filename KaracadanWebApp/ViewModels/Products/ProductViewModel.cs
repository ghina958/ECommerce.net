using KaracadanWebApp.Data;
using KaracadanWebApp.Models;

namespace KaracadanWebApp.ViewModels.Products
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            productSearchViewModel = new ProductSearchViewModel();
            products = new List<Product>();
        }


        public List<Product>? products { get; set; }

        public ProductSearchViewModel? productSearchViewModel { get; set; }



    }
}
