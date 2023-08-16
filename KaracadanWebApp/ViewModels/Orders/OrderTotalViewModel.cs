using KaracadanWebApp.Data;
using KaracadanWebApp.Models;

namespace KaracadanWebApp.ViewModels.Orders
{
    public class OrderTotalViewModel
    {
        public OrderTotalViewModel()
        {
            OrderDetail = new List<OrderDetail>();
        }

        public int Id { get; set; }
        public List<OrderDetail>? OrderDetail { get; set; }
        public int No { get; set; }
        public int TheTotalPrice { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string? ApplicationUser { get; set; }

    }
}
