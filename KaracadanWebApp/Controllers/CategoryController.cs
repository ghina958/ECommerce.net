using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using KaracadanWebApp.Repository;
using KaracadanWebApp.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KaracadanWebApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        #region Ctor
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        #endregion

        #region Area for admin  to CRUD Operations
        public async Task <IActionResult> Index()
        {
            var allcategory = await _categoryRepository.GetAllCategories();
            ViewBag.BreadCrumbFirstItem = "Category List";
            ViewBag.BreadCrumbFirstItemLink = "/category";
            var model = new CategoryViewModel()
            {
                Categories = allcategory
            };
            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> Index(CategorySearchViewModel categorySearchViewModel)
        {
            var allcategory = await _categoryRepository.GetAllCategories(categorySearchViewModel);
            var model = new CategoryViewModel()
            {
                Categories = allcategory
            };         
            return View(model);
        }


        [Authorize(Roles = "admin")]
        public async Task <IActionResult> Create() 
        {
               var model = new CreateCategoryViewModel();            
               ViewBag.Title = "Add" + model.Title;
               ViewBag.BreadCrumbFirstItem = "Category List";
               ViewBag.BreadCrumbFirstItemLink = "/category";
               ViewBag.BreadCrumbSecondItem = "Add";

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel createCategoryVM)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = createCategoryVM.Title

                };
                _categoryRepository.Add(category);
                return RedirectToAction("Index");
            }
            return View(createCategoryVM);
        }


        [Authorize(Roles = "admin")]
        public async Task< ActionResult> Edit(int id) 
        {
            var cat = await _categoryRepository.GetCategoryById(id);
            if (cat == null) return View("Error");
            ViewBag.Title = "Edit " + cat.Name;
            ViewBag.BreadCrumbFirstItem = "Category List";
            ViewBag.BreadCrumbFirstItemLink = "/category";
            ViewBag.BreadCrumbSecondItem = "Edit";

            var categoryVM = new EditCategoryViewModel
            {
                Title= cat.Name

            };
            return View(categoryVM);
        }

        [HttpPost]   
        public async Task<ActionResult> Edit(int id ,EditCategoryViewModel editCategoryViewModel)
        {
            //check the valditiy of miodel
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "failed to edit category");
                return View("Edit", editCategoryViewModel);
            }
            // model entity mappers
            
            // create new category instance to be updated 
            var category = new Category 
            {
                Id = id,
                Name = editCategoryViewModel.Title 
            };
            // update the category instance
            _categoryRepository.Update(category);

            //redirect to the index view 
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var cat = await _categoryRepository.GetCategoryById(id);
            if (cat == null) return View("Error");

            ViewBag.Title = "Delete " + cat.Name;
            ViewBag.BreadCrumbFirstItem = "Category List";
            ViewBag.BreadCrumbFirstItemLink = "/category";
            ViewBag.BreadCrumbSecondItem = "Delete";
            return View(cat);
        }

        [HttpPost , ActionName("Delete")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var cat = await _categoryRepository.GetCategoryById(id);
            if (cat == null) { return View("Error"); }

            _categoryRepository.Delete(cat);
            return RedirectToAction("Index");
        }


        #endregion
    }
}
