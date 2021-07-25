using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InventoryWebApplication.Views.Authentication
{
    public class LoginModel : PageModel
    {
        public bool FailedAuthentication { get; set; }
    }
}