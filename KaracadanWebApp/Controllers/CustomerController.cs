using KaracadanWebApp.Interfaces;
using KaracadanWebApp.ViewModels.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace KaracadanWebApp.Controllers
{
    public class CustomerController : Controller
    {
        #region Ctor
        private readonly ICustomerRepository _customerRepository;
        
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        #endregion

        #region Admin Area to get all logged users and CRUD operation on customers

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var allCustomers = await _customerRepository.GetAll();
            ViewBag.BreadCrumbFirstItem = "Customer List";
            ViewBag.BreadCrumbFirstItemLink = "/customer";
            var model = new CustomerViewModel()
            {
               Users=allCustomers,

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(CustomerSearchViewModel customerSearchViewModel)
        {
            var allCustomers = await _customerRepository.GetAll(customerSearchViewModel);
            var model = new CustomerViewModel()
            {
                Users = allCustomers,

            };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var editUser = await _customerRepository.GetByIdAsync(id);
            if (editUser == null) { return View("Error");  }

            var Editmodel = new EditCustomerViewModel()
            {           
                CustomerName = editUser.UserName,
            };
            return View(Editmodel);
        
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id,EditCustomerViewModel editCustomerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit Customer Name");
                return View("Edit", editCustomerViewModel);
            }
            var customer = await _customerRepository.GetByIdAsync(id);       
            customer.UserName = editCustomerViewModel.CustomerName;

            _customerRepository.Update(customer);
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return View("Error");

            ViewBag.Title = "Delete " + customer.UserName;
            ViewBag.BreadCrumbFirstItem = "Customer List";
            ViewBag.BreadCrumbFirstItemLink = "/customer";
            ViewBag.BreadCrumbSecondItem = "Delete";
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) { return View("Error"); }

            _customerRepository.Delete(customer);
            return RedirectToAction("Index");
        }

        #endregion

    }
}
