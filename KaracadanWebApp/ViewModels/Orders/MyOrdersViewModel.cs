using KaracadanWebApp.Data;
using KaracadanWebApp.Models;

namespace KaracadanWebApp.ViewModels.Orders
{
    public class MyOrdersViewModel
    {

        public MyOrdersViewModel()

        {
            allMyPervious = new List<AllMyPerviousOrders>();
        }

        public List<AllMyPerviousOrders>? allMyPervious { get; set; }
    }
}
