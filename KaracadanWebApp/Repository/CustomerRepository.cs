using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.Customers;
using Microsoft.EntityFrameworkCore;

namespace KaracadanWebApp.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;
        public CustomerRepository(ApplicationDbContext context) 
        {
            _context = context;

        }
        public bool Add(ApplicationUser appCustomer)
        {
            _context.Users.Add(appCustomer);
            return Save();
        }
    
        public bool Delete(ApplicationUser appCustomer)
        {
            _context.Users.Remove(appCustomer);
            return Save();
        }

        public async Task<List<ApplicationUser>> GetAll(CustomerSearchViewModel? customerSearchViewModel = null)
        {
            if (customerSearchViewModel == null)
            {
                return await _context.Users.ToListAsync();
            }
            var result = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(customerSearchViewModel.UserName))
            {
                result = result.Where(x => x.UserName == customerSearchViewModel.UserName);
            }
            return await result.ToListAsync();
        }

        public async Task<ApplicationUser?> GetByIdAsync(string id)
        {
            return  _context.Users.FirstOrDefault(i=>i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(ApplicationUser appCustomer)
        {
            _context.Users.Update(appCustomer);
            return Save();
        }
    }
}
