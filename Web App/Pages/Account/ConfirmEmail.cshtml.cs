using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_App.Pages.Account;

public class ConfirmEmail : PageModel
{
    private readonly UserManager<IdentityUser> userManager;

    [BindProperty] public string Message { get; set; }
    public async Task<IActionResult> OnGetAsync(string userId, string token)
    {
        var user = await this.userManager.FindByIdAsync(userId);
        if (user == null)
        {
            ModelState.AddModelError("UserNotFound", "The user you specified was not found in the database");
            Message = "Failed to validate user email";
            return Page();
        }

        var userConfirmation = await this.userManager.ConfirmEmailAsync(user, token);
        if (!userConfirmation.Succeeded)
        {
            ModelState.AddModelError("Fail", "Failed to confirm user email");
            return Page();
        }
        return Redirect("/Index");
    }


    public ConfirmEmail(UserManager<IdentityUser> userManager)
    {
        this.userManager = userManager;
    }
}