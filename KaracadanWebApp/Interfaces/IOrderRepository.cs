using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.Orders;

namespace KaracadanWebApp.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrders(OrderSearchViewModel? orderSearchViewModel=null);
        Task<Order> GetOrderById(int id);
        Task<List<Order>> GetOrderDetailByOrderStatues(string userId, int statusId);
        //Task<Order?> GetOrderByOrderNo(int id);
        bool Add(Order order);
        bool Update(Order order);
        bool Delete(Order order);
        bool Save();
        
    }
}
