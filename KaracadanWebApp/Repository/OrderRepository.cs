using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.Orders;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace KaracadanWebApp.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) 
        {
            _context = context;
        } 
        public bool Add(Order order)
        {
            _context.Add(order);
            return Save();
        }

        public bool Delete(Order order)
        {
            _context.Remove(order);
            return Save();
        }

        public async Task<List<Order>> GetAllOrders(OrderSearchViewModel? orderSearchViewModel=null)
        {
            if (orderSearchViewModel == null)
            {
                return await _context.Orders.Include(i => i.ApplicationUser).Include(x => x.OrderDetails).ToListAsync();
            }
            else
            {
                await _context.Orders.Include(i => i.ApplicationUser).Include(x => x.OrderDetails).ToListAsync();
                var result = _context.Orders.Include(i => i.ApplicationUser).AsQueryable();

                if (orderSearchViewModel.No != 0)
                {
                    result = result.Where(x => x.No == orderSearchViewModel.No);
                }

                if (!string.IsNullOrEmpty(orderSearchViewModel.ApplicationUserId))
                {
                    result = result.Where(x => x.ApplicationUser.UserName == orderSearchViewModel.ApplicationUserId);
                }

                if (orderSearchViewModel.OrderStatus != 0)
                {
                    result = result.Where(x => x.StatusId == orderSearchViewModel.OrderStatus);
                }

                return await result.ToListAsync();
            }

        }
        public async Task<Order?> GetOrderById(int id)
        {
            return await _context.Orders.Include(i => i.ApplicationUser).FirstOrDefaultAsync(i => i.Id == id);
        }

        //public async Task<Order?> GetOrderByOrderNo(int id)
        //{
        //    return await _context.Orders.FirstOrDefaultAsync(i => i.Id == id);
        //}

        public async Task<List<Order>> GetOrderDetailByOrderStatues(string userId, int statusId)
        {
            var query= await _context.Orders.Where(x => x.ApplicationUserId.Equals(userId) && x.StatusId == statusId).Include(x=> x.OrderDetails).ToListAsync();
            foreach (var item in query)
            {
                foreach (var aa in item.OrderDetails)
                {
                    aa.Product= _context.Products.First(a=> a.Id== aa.ProductId);
                }
            }
            return query;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Order order)
        {
            _context.Update(order);
            return Save();
        }
    }
}
