using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn_Asp_Net_Identity.Pages.Account
{
    public class SignOutModel : PageModel
    {
        public void OnGet()
        {
            // await HttpContext.SignOutAsync("MyCookie");
            // return Redirect("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync("MyCookie");
            return Redirect("/Index");
        }
    }
}
