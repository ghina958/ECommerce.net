using KaracadanWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.ViewModels.Products;
using KaracadanWebApp.ViewModels.Categories;
namespace KaracadanWebApp.Controllers
{

    public class HomeController : Controller
    {
        #region Fields

        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        #endregion


        #region Ctor
        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;   
        }
        #endregion


        #region StartPage of project
        public async Task<IActionResult> Index()
        {
            var allProduct = await _productRepository.GetAll();           
            var model = new ProductViewModel()
            {
                products = allProduct
                
            };
            ViewBag.BreadCrumbFirstItem = "Dashboard";
            ViewBag.BreadCrumbSecondItem = "";
            return View(model);
        }
        #endregion


        #region Navbar in Home Page

        //[HttpPost]
        public async Task<IActionResult> Search(string name)
        {
             
            var allProduct = await _productRepository.GetAll(new ProductSearchViewModel { Name = name});
            var model = new ProductViewModel()
            {
                products = allProduct

            };
            return View("Index",model);
        }


        public async Task<IActionResult> DropdownCategories()
        {
            var allcategory = await _categoryRepository.GetAllCategories();
            var model = new CategoryViewModel()
            {
                Categories = allcategory
            };
         
            return PartialView("LayoutCategoriesDropList", model);

        }

        public async Task<IActionResult> DropdownItems(int id)
        {          
            var allProduct = await _productRepository.GetAll(new ProductSearchViewModel { CategoryId = id });
            
            var model = new ProductViewModel()
            {
                products = allProduct
            };
           
            return View("Index", model);
        }
        #endregion

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}