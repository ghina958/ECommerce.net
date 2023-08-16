using CloudinaryDotNet.Actions;
using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels;
using KaracadanWebApp.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace KaracadanWebApp.Controllers
{
    public class OrderController : Controller
    {
        #region Fields
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;
        #endregion

        #region Ctor
        public OrderController(IOrderRepository orderRepository, 
            IOrderDetailsRepository orderDetailsRepository, 
            IHttpContextAccessor httpContextAccessor, 
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
        }
        #endregion

        #region Public Area of cart addItem and deleteItem and previous orders 

        [Authorize]
        public async Task<ActionResult> AddItemCart(int id)
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var product = await _productRepository.GetProductByIdAsync(id);
            //my cart function that get current user with his orders in cart statues
            var myCart = (await _orderRepository.GetOrderDetailByOrderStatues(curUserID, (int)OrderStatus.Cart)).FirstOrDefault();

            if (myCart == null)
            {
                //if we dont have order in cart create new order and add OrderItems to their
                var rnd = new Random();
                var order = new Order
                {
                    No = rnd.Next(1000, 9999),
                    Date = DateTime.Now,
                    StatusId = (int)OrderStatus.Cart,
                    OrderStatus = OrderStatus.Cart,
                    ApplicationUserId = curUserID

                };
                _orderRepository.Add(order);

                var orderDt = new OrderDetail
                {
                    Id = order.Id,
                    ProductId = id,
                    OrderId = order.Id,
                    Quantity = 1,
                    PriceProductOrderDetail = product.Price,
                };

                _orderDetailsRepository.Add(orderDt);
            }
            else
            {
                //if we have already order and items in 
                var previousOrderDetail = await _orderDetailsRepository.GetOrderDetailByProductId(myCart.Id, id);
                if (previousOrderDetail == null)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = id,
                        OrderId = myCart.Id,
                        Quantity = 1,
                        PriceProductOrderDetail = product.Price,
                    };

                    _orderDetailsRepository.Add(orderDetail);
                }
                //if we have same item in cart increase quantity
                else
                {
                    previousOrderDetail.Quantity += 1;
                    _orderDetailsRepository.Update(previousOrderDetail);
                }
            }
            return RedirectToAction("MyCart");
        }



        [Authorize(Roles = "user")]
        public async Task<ActionResult> MyCart()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            //Get all orders that in cart statues
            var myCart = (await _orderRepository.GetOrderDetailByOrderStatues(curUserID, (int)OrderStatus.Cart)).FirstOrDefault();
            if (myCart == null) return View(new MyCartViewModel());

            var model = new MyCartViewModel()
            {
                No = myCart.No,
                Date = myCart.Date,
                UserId = curUserID,
            };

            model.Details = myCart.OrderDetails.ToList();
            foreach (var item in model.Details)
            {
                model.TheTotalPrice += item.PriceProductOrderDetail * item.Quantity;
            }
            return View(model);
        }


        public async Task<ActionResult> SubmittingButtonForOrder()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var myCart = (await _orderRepository.GetOrderDetailByOrderStatues(curUserID, (int)OrderStatus.Cart)).FirstOrDefault();

            //change the statues of order from cart to submmitted then update it
            myCart.StatusId = (int)OrderStatus.Submitted;
            myCart.OrderStatus = OrderStatus.Submitted;
            _orderRepository.Update(myCart);
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "user")]
        public async Task<ActionResult> MyOrders()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            //Get form db all submitted oreders
            var OrdersSubmitted = (await _orderRepository.GetOrderDetailByOrderStatues(curUserID, (int)OrderStatus.Submitted));
            if (OrdersSubmitted == null) return View(new MyOrdersViewModel());

            var model = new MyOrdersViewModel();

            foreach(var order in  OrdersSubmitted)
            {
                var previousOrder = new AllMyPerviousOrders()
                {
                    No=order.No,
                    Date=order.Date,
                };

                var Sum = 0;
                foreach (var item in order.OrderDetails)
                {
                    Sum += item.PriceProductOrderDetail * item.Quantity;
                }

                previousOrder.TheTotalPrice = Sum;
                previousOrder.Details = order.OrderDetails.ToList();
                model.allMyPervious.Add(previousOrder);

            }
           
            return View(model);
        }


        [Authorize(Roles = "user")]
        public async Task<ActionResult> DeleteItemCart(int id)
        {
            //delete item from basket by OrderDetail Id
            var deleteItem = await _orderDetailsRepository.GetOrderDetailById(id);
            if (deleteItem == null) { return View("Error"); }
            _orderDetailsRepository.Delete(deleteItem);
            return RedirectToAction("MyCart");
        }

        #endregion

        #region Admin Area

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var allOrders = await _orderRepository.GetAllOrders();
            ViewBag.BreadCrumbFirstItem = "Order List";
            ViewBag.BreadCrumbFirstItemLink = "/order";

            var model = new OrderViewModel();
            foreach (var order in allOrders)
            {
                var orderTotal = new OrderTotalViewModel()
                {
                    Id = order.Id,
                    No = order.No,
                    ApplicationUser = order.ApplicationUser.UserName,
                    Date = order.Date,
                    OrderStatus = order.OrderStatus,
                };
                var sum = 0;

                foreach (var orderItem in order.OrderDetails)
                {
                    sum += orderItem.Quantity * orderItem.PriceProductOrderDetail;
                }
                orderTotal.TheTotalPrice = sum;
                model.OrdersTotal.Add(orderTotal);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(OrderSearchViewModel orderSearchViewModel)
        {
            var allOrders = await _orderRepository.GetAllOrders(orderSearchViewModel);
            var model = new OrderViewModel();
            foreach (var order in allOrders)
            {
                var orderTotal = new OrderTotalViewModel()
                {
                    Id = order.Id,
                    No = order.No,
                    ApplicationUser = order.ApplicationUser.UserName,
                    Date = order.Date,
                    OrderStatus = order.OrderStatus,

                };
                var sum = 0;

                foreach (var orderDetail in order.OrderDetails)
                {
                    sum += orderDetail.Quantity * orderDetail.PriceProductOrderDetail;
                }
                orderTotal.TheTotalPrice = sum;
                model.OrdersTotal.Add(orderTotal);
            }
            return View(model);
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            { return View("Error"); }

            ViewBag.BreadCrumbFirstItem = "Order List";
            ViewBag.BreadCrumbFirstItemLink = "/order";
            ViewBag.BreadCrumbSecondItem = "Edit";


            var orderVM = new EditOrderViewModel
            {
                Id = id,
                No = order.No,
                Date = order.Date,
                OrderStatus = order.OrderStatus,
                ApplicationUserId = order.ApplicationUserId,
            };

            return View(orderVM);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(int id, EditOrderViewModel editOrderViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit Order");
                return View("Edit", editOrderViewModel);
            }

            var Order = new Order
            {
                Id = id,
                No = editOrderViewModel.No,
                Date = editOrderViewModel.Date,
                OrderStatus = editOrderViewModel.OrderStatus,
                ApplicationUserId = editOrderViewModel.ApplicationUserId,

            };

            _orderRepository.Update(Order);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null) return View("Error");

            ViewBag.Title = "Delete " + order.No;
            ViewBag.BreadCrumbFirstItem = "Orders List";
            ViewBag.BreadCrumbFirstItemLink = "/orders";
            ViewBag.BreadCrumbSecondItem = "Delete";
            return View(order);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null) { return View("Error"); }

            _orderRepository.Delete(order);
            return RedirectToAction("Index");
        }
        #endregion

    }
}
