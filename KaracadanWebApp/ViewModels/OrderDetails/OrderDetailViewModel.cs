using KaracadanWebApp.Models;

namespace KaracadanWebApp.ViewModels.OrderDetails
{
    public class OrderDetailViewModel
    {
        public OrderDetailViewModel()
        {
            orderDetails = new List<OrderDetail>();
        }


        public List<OrderDetail>? orderDetails { get; set; }
        public int TotalPrice { get; set; }
        public int OrderNo { get; set; }
        public string OrderUser { get; set; }

    }
}
