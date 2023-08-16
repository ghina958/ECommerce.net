using System.ComponentModel.DataAnnotations;

namespace KaracadanWebApp.ViewModels.UsersAccountViewModel
{
    public class UserLoginModel
    {
        public bool RememberMe { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is required")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
