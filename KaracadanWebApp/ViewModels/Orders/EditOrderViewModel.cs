using KaracadanWebApp.Data;

namespace KaracadanWebApp.ViewModels.Orders
{
    public class EditOrderViewModel
    {
        public int Id { get; set; }
        public int No { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
