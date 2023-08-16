using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Repository;
using KaracadanWebApp.ViewModels.DashBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KaracadanWebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class DashBoardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        #region Ctor
        public DashBoardController(IDashboardRepository dashboardRepository) 
        {
            _dashboardRepository=dashboardRepository;
        }
        #endregion

        #region Admin Area to show count every table 

        public async Task<IActionResult> Index()
        {

            var countOfAllCategories = await _dashboardRepository.GetCategoriesCount();
            var countOfAllCustomers = await _dashboardRepository.GetCustomersCount();
            var countOfAllOrders = await _dashboardRepository.GetOrdersCount();
            var countOfAllProducts = await _dashboardRepository.GetProductCount();

            var CountVM =new DashBoardViewModel()
            { 
                CategoriesAmount = countOfAllCategories,
                CustomerAmount = countOfAllCustomers,
                OrdersAmount = countOfAllOrders,
                ProductAmount = countOfAllProducts ,

            
            };
            return View(CountVM);
        }
        #endregion
    }
}
