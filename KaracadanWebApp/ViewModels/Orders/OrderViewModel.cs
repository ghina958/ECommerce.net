using KaracadanWebApp.Models;

namespace KaracadanWebApp.ViewModels.Orders
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderSearchViewModel = new OrderSearchViewModel();
            OrdersTotal = new List<OrderTotalViewModel>();

        }
        public List<OrderTotalViewModel>? OrdersTotal { get; set; }
        public OrderSearchViewModel? OrderSearchViewModel { get; set; }
    }
}
