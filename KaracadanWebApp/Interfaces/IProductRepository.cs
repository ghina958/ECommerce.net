using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.Products;

namespace KaracadanWebApp.Interfaces
{
    public interface IProductRepository
    {
             Task<List<Product>> GetAll(ProductSearchViewModel? productSearchViewModelproductSearchViewModel = null);
             Task<Product?> GetProductByIdAsync(int id);       
             Task<Product?> GetByIdAsyncNoTracking(int id);           
             bool Add(Product product);
             bool Update(Product product);
             bool Delete(Product product);
             bool Save();
    }
}
