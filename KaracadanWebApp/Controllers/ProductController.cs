using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using KaracadanWebApp.Repository;
using KaracadanWebApp.Services;
using KaracadanWebApp.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KaracadanWebApp.Controllers
{
    public class ProductController : Controller
    {
        #region Fields
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPhotoService _photoService;
        #endregion

        #region Ctor
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository,IPhotoService photoService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _photoService= photoService;
        }
        #endregion



        public async Task<IActionResult> Index()
        {
            var allProduct = await _productRepository.GetAll();
            ViewBag.BreadCrumbFirstItem = "Products List";
            ViewBag.BreadCrumbFirstItemLink = "/product";

            var model = new ProductViewModel()
            {
                products = allProduct
            };
            var categories = await _categoryRepository.GetAllCategories();
            foreach (var item in categories)
            {
                model.productSearchViewModel!.AvailableCategories.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    }
                    );
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductSearchViewModel productSearchViewModel)
        {
            var allProduct = await _productRepository.GetAll(productSearchViewModel);         
            var model = new ProductViewModel()
            {
                products = allProduct
              
            };
            var categories = await _categoryRepository.GetAllCategories();
            foreach (var item in categories)
            {
                model.productSearchViewModel!.AvailableCategories.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    }
                    );
            }
            return View(model);

        }


        #region Admin Area to CRUD operation on products

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateProductViewModel();          
            //ViewBag.Title = "Add" + model.Name;
            ViewBag.BreadCrumbFirstItem = "Product List";
            ViewBag.BreadCrumbFirstItemLink = "/product";
            ViewBag.BreadCrumbSecondItem = "Add";
            var categories = await _categoryRepository.GetAllCategories();
            foreach (var item in categories)
            {
                model.Categories.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    }
                    );
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                                                //AddPhotoAsync(IFormFile file)
                var result = await _photoService.AddPhotoAsync(productVM.Image);

                var product = new Product
                {
                    Name = productVM.Name,
                    Description = productVM.Description,
                    Image=result.Url.ToString(),
                    Price = productVM.Price,
                    ProductsStatus = productVM.ProductsStatus,                  
                    CategoryId = productVM.CategoryId

                };
                _productRepository.Add(product);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }
            return View(productVM);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return View("Error");
            ViewBag.Title = "Edit " + product.Name;
            ViewBag.BreadCrumbFirstItem = "Products List";
            ViewBag.BreadCrumbFirstItemLink = "/product";
            ViewBag.BreadCrumbSecondItem = "Edit";
            
            var productVM = new EditProductViewModel
            {
                Name = product.Name,
                Description = product.Description,
                URL= product.Image,  
                Price = product.Price,
                ProductsStatus = product.ProductsStatus,
                CategoryId = product.CategoryId,
                
            };
            var categories = await _categoryRepository.GetAllCategories();
            foreach (var item in categories)
            {
                productVM.Categories.Add(
                    new SelectListItem
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    }
                    );
            }
            return View(productVM);
        }

        [HttpPost]  
        public async Task<IActionResult> Edit(int id, EditProductViewModel productVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit product");
                return View("Edit", productVM);
            }
            var ProductEdit = await _productRepository.GetByIdAsyncNoTracking(id);

            if (ProductEdit == null)
            {
                return View("Error");
            }
            var photoResult = await _photoService.AddPhotoAsync(productVM.Image);

            if(photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(productVM);

            }

            if (!string.IsNullOrEmpty(ProductEdit.Image))
            {
                _ = _photoService.DeletePhotoAsync(ProductEdit.Image);
            }

                var product = new Product
                {
                    Id = id,
                    Name = productVM.Name,
                    Description = productVM.Description,
                    Image = photoResult.Url.ToString(),
                    Price = productVM.Price,
                    ProductsStatus = productVM.ProductsStatus,
                    CategoryId = productVM.CategoryId,
                    
                };
                _productRepository.Update(product);
                return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var productDetails = await _productRepository.GetProductByIdAsync(id);
            if (productDetails == null) return View("Error");
            ViewBag.Title = "Delete " + productDetails.Name;
            ViewBag.BreadCrumbFirstItem = "Products List";
            ViewBag.BreadCrumbFirstItemLink = "/product";
            ViewBag.BreadCrumbSecondItem = "Delete";
            return View(productDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productDetails = await _productRepository.GetProductByIdAsync(id);

            if (productDetails == null)
            {
                return View("Error");
            }
            _productRepository.Delete(productDetails);
            return RedirectToAction("Index");
        }

        #endregion

    }

}
