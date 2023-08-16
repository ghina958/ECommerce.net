using System.ComponentModel.DataAnnotations;

namespace KaracadanWebApp.ViewModels.UsersAccountViewModel
{
    public class UserRegisterModel
    {

        [Display(Name = "Email Adress")]
        [Required(ErrorMessage = "Email Adres is Required")]
        public string EmailAdress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
