using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KaracadanWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;


        public DashboardRepository(ApplicationDbContext context) 
        {
           _context=context;
        }

        public async Task<int> GetCategoriesCount()
        {
            var count = _context.Categories.Count();

            return count;
        }

        public async Task<int> GetCustomersCount()
        {
            var count = _context.Users.Count();

            return count;
        }

        public async Task<int> GetOrdersCount()
        {
            var count = _context.Orders.Count();

            return count;
        }

        public async Task<int> GetProductCount()
        {
            var count =  _context.Products.Count();
                
            return count;
        }


    }
}
