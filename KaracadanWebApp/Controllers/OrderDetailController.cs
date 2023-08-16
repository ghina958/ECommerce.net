using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels.OrderDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KaracadanWebApp.Controllers
{
   
    public class OrderDetailController : Controller
    {
        #region Fields
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IOrderRepository _orderRepository;
        #endregion

        #region Ctor
        public OrderDetailController(IOrderDetailsRepository orderDetailsRepository,IOrderRepository orderRepository)
        {
            _orderDetailsRepository = orderDetailsRepository;
            _orderRepository=orderRepository;
        }
        #endregion

        #region Admin Area
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            List<OrderDetail> orderDetails = await _orderDetailsRepository.GetAllOrderDetails();
            return View(orderDetails);      
        }


        [HttpGet]
        public async Task<IActionResult> ItemsOfOrder(int id)
        {
            var detail = await _orderDetailsRepository.GetOrderDetailByOrderId(id);
            var OrderNo= await _orderRepository.GetOrderById(id);
            var OrderUserName= await _orderRepository.GetOrderById(id);


            if (detail == null) { return View("Error"); }
            var total = 0;
            foreach (var item in detail)
            {
                total += item.PriceProductOrderDetail * item.Quantity;
            }

            var model = new OrderDetailViewModel()
            {            
                OrderNo = OrderNo.No,
                TotalPrice = total,
                OrderUser= OrderUserName.ApplicationUser.UserName,             
            };

            model.orderDetails.AddRange(detail);
            //orderDetails here is generic list 
            //AddRange(): will add elements of specified collection (orderDetails) to the end of the list(detail)
            return View(model);
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int id)
        {
            var OrderItem = await _orderDetailsRepository.GetOrderDetailById(id);     
            if (OrderItem == null) return View("Error");
            ViewBag.Title = "Edit " + OrderItem.Product.Name;
            ViewBag.BreadCrumbFirstItem = "OrderDetail List";
            ViewBag.BreadCrumbFirstItemLink = "/OrderDetail";
            ViewBag.BreadCrumbSecondItem = "Edit";

            var detailVM = new EditOrderDetailViewModel
            {
                Id = id,
                Quantity = OrderItem.Quantity,
                PriceProductOrderDetail= OrderItem.PriceProductOrderDetail,
                ProductId = OrderItem.ProductId,
                OrderId = OrderItem.OrderId     
            };
            return View(detailVM); 
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id,EditOrderDetailViewModel editOrderDetailViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit OrderDetail");
                return View("Edit", editOrderDetailViewModel);
            }
            var OrderDetail = new OrderDetail
            {
                Id = id,
                Quantity = editOrderDetailViewModel.Quantity,
                PriceProductOrderDetail=editOrderDetailViewModel.PriceProductOrderDetail,
                ProductId = editOrderDetailViewModel.ProductId,
                OrderId=editOrderDetailViewModel.OrderId
                
            };
            _orderDetailsRepository.Update(OrderDetail);
            return RedirectToAction("Detail", new {id= editOrderDetailViewModel.OrderId }); 

        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var OrderItem = await _orderDetailsRepository.GetOrderDetailById(id);
            if (OrderItem == null) return View("Error");

            ViewBag.Title = "Delete " + OrderItem.Product.Name;
            ViewBag.BreadCrumbFirstItem = "OrderDetail List";
            ViewBag.BreadCrumbFirstItemLink = "/OrderDetail";
            ViewBag.BreadCrumbSecondItem = "Delete";
            return View(OrderItem);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteDetails(int id)
        {
            var OrderItem = await _orderDetailsRepository.GetOrderDetailById(id);
            if (OrderItem == null) { return View("Error"); }

            _orderDetailsRepository.Delete(OrderItem);
            return RedirectToAction("Index", new { id });
        }

        #endregion
    }
}
