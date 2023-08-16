using KaracadanWebApp.Models;

namespace KaracadanWebApp.ViewModels.Customers
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            Users = new List<ApplicationUser>();
            CustomerSearchViewModel = new CustomerSearchViewModel();

        }
        public List<ApplicationUser>? Users { get; set; }
        public CustomerSearchViewModel? CustomerSearchViewModel { get; set; }

        //public string Id { get; set; }
        //public string UserName { get; set; }
    }
}
