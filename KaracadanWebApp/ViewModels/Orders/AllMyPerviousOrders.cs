using KaracadanWebApp.Models;

namespace KaracadanWebApp.ViewModels.Orders
{
    public class AllMyPerviousOrders
    {
        public AllMyPerviousOrders()
        {
            Details = new List<OrderDetail>();
        }
        public int No { get; set; }
        public DateTime Date { get; set; }
        public decimal TheTotalPrice { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}
