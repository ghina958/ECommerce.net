using KaracadanWebApp.Models;
using System.Diagnostics;

namespace KaracadanWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<int> GetProductCount();
        Task<int> GetOrdersCount();
        Task<int> GetCategoriesCount();

        Task<int> GetCustomersCount();

    }

}