using KaracadanWebApp.Data;
using KaracadanWebApp.Interfaces;
using KaracadanWebApp.Models;
using KaracadanWebApp.ViewModels;
using KaracadanWebApp.ViewModels.UsersAccountViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace KaracadanWebApp.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomerRepository _customerRepository;
        #endregion

        #region Ctor
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ICustomerRepository customerRepository)

        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _customerRepository = customerRepository;
        }
        #endregion

        #region Login and Sign in Area
        public ActionResult Login()
        {
            var response = new UserLoginModel();
            return View(response);
        }


        [HttpPost]
        public async Task<ActionResult> Login(UserLoginModel userLoginModel)
        {
            if (!ModelState.IsValid) return View(userLoginModel);

            var user = await _userManager.FindByEmailAsync(userLoginModel.Email);
            
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, userLoginModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                //password is incorrect
                TempData["Error"] = "Wrong credintails please try again ";
                return View(userLoginModel);
            }
            //User not found
            TempData["Error"] = "Wrong credintails please try again ";
            return View(userLoginModel);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            var response = new UserRegisterModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserRegisterModel userRegisterModel)
        {
            if (!ModelState.IsValid) return View(userRegisterModel);

            var user = await _userManager.FindByEmailAsync(userRegisterModel.EmailAdress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(userRegisterModel);
            }
            var newUser = new ApplicationUser()
            {
                Email = userRegisterModel.EmailAdress,
                UserName = userRegisterModel.EmailAdress,

            };

            IdentityResult result = await _userManager.CreateAsync(newUser, userRegisterModel.Password);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<ActionResult> CurrentLoggedUser()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var loggedUser = await _customerRepository.GetByIdAsync(curUserID);
            if (loggedUser == null) return View("Login");

            var model = new CurrentLoggedUserViewModel()
            {
                UserId = loggedUser.Id,
                UserEmail = loggedUser.Email,
                UserName = loggedUser.UserName
            };

            return View(model);
        }

            public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ForgetPassword()
        {
            ViewBag.Success = false;
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(UserLoginModel userLoginModel)
        {
            //Add reset password logic here
            throw new NotImplementedException();
     
        }

        #endregion
    }
}
