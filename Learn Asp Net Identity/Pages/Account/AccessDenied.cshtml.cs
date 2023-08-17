using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn_Asp_Net_Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync("MyCookie");
            return RedirectToPage("/Index");
        }


    }
}
