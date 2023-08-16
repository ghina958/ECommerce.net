using KaracadanWebApp.Models;

namespace KaracadanWebApp.Interfaces
{
    public interface IOrderDetailsRepository
    {
        Task<List<OrderDetail>> GetAllOrderDetails();
        Task<OrderDetail> GetOrderDetailById(int id);
        Task<OrderDetail> GetOrderDetailByProductId(int orderId, int productId);
        bool Add(OrderDetail orderDetail);
        bool Update(OrderDetail orderDetail);
        bool Delete(OrderDetail orderDetail);
        bool Save();
        Task<List<OrderDetail>> GetOrderDetailByOrderId(int orderId);
    }
}
