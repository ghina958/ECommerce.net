using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.Customers;

namespace KaracadanWebApp.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<ApplicationUser>> GetAll(CustomerSearchViewModel? customerSearchViewModel=null);
        Task<ApplicationUser?> GetByIdAsync(string id);
        bool Add(ApplicationUser appCustomer);
        bool Update(ApplicationUser appCustomer);
        bool Delete(ApplicationUser appCustomer);
        bool Save();
    }
}
