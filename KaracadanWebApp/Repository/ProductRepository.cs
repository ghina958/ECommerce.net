using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.Products;
using Microsoft.EntityFrameworkCore;

namespace KaracadanWebApp.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public async Task<List<Product>> GetAll(ProductSearchViewModel? productSearchViewModel = null)
        {
            
            if (productSearchViewModel == null)
            {
                return await _context.Products.Include(c => c.Category).ToListAsync();
            }


            var result = _context.Products.Include(c => c.Category).AsQueryable();

            if (!string.IsNullOrEmpty( productSearchViewModel.Name))
            {
                result = result.Where(x=>x.Name == productSearchViewModel.Name);
            }

            if (!string.IsNullOrEmpty(productSearchViewModel.Description))
            {
                result = result.Where(x => x.Description == productSearchViewModel.Description);
            }

            if (productSearchViewModel.CategoryId != 0)
            {
                result = result.Where(x => x.CategoryId == productSearchViewModel.CategoryId);
            }

            if (productSearchViewModel.Price != 0)
            {
                result = result.Where(x => x.Price == productSearchViewModel.Price);
            }
            if (productSearchViewModel.ProductsStatus != 0)
            {
                result = result.Where(x => x.StatusId == productSearchViewModel.ProductsStatus);
            }

            return await result.ToListAsync();

        }
 
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Product?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Products.Include(i => i.Category).Include(i=> i.OrderDetails).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Update(Product product)
        {
            _context.Update(product);
            return Save();
        }
        public bool Add(Product product)
        {
            _context.Add(product);
            return Save();
        }

        public bool Delete(Product product)
        {
            _context.Remove(product);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
       

    }
}
