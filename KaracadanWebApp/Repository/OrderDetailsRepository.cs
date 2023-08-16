using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KaracadanWebApp.Repository
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext context)      
        {
            _context = context;
        }
        public bool Add(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            return Save();
        }

        public bool Delete(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
            return Save();
        }

        public async Task<List<OrderDetail>> GetAllOrderDetails()
        {         
           return await _context.OrderDetails.Include(i => i.Product).ToListAsync();

        }    
        //this function check if the cart have same this adding Product to increase the quantity
        public async Task<OrderDetail> GetOrderDetailByProductId(int orderId,int productId)
        {
            return await _context.OrderDetails.Where(x => x.ProductId==productId && x.OrderId== orderId).FirstOrDefaultAsync();
        }

        public async Task<OrderDetail> GetOrderDetailById(int id)
        {
            return await _context.OrderDetails.Include(i => i.Product).Include(i => i.Order).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<OrderDetail>> GetOrderDetailByOrderId(int orderId)
        {
            return await _context.OrderDetails.Where(i=> i.OrderId == orderId).Include(i => i.Product).Include(i => i.Order).ToListAsync();
           
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(OrderDetail orderDetail)
        {
           _context.OrderDetails.Update(orderDetail);
            return Save();
        }
    }
}
