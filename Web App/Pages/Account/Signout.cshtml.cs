using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_App.Pages.Account;

public class Signout : PageModel
{
    
        public void OnGet()
        {
            // await HttpContext.SignOutAsync("MyCookie");
            // return Redirect("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }
    
}