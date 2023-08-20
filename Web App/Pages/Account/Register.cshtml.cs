using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_App.Pages.Account;

public class Register : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    [BindProperty] public RegisterViewModel RegisterViewModel { get; set; }


    public Register(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }


    public void OnGet()
    {
    }

    string SplitEmail(string email)
    {
        var emailSplit = email.Split("@");
        return emailSplit.First();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        //validate email address (optional --  we have the code to check for that uniqueness of the email)
        //create user if email is valid 
        var identityUser = new IdentityUser
        {
            Email = RegisterViewModel.Email,
            UserName = SplitEmail(RegisterViewModel.Email),
        };
        var result = await _userManager.CreateAsync(identityUser, RegisterViewModel.Password);
        if (result.Succeeded)
        {
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            return RedirectToPage("/Account/ConfirmEmail",
                new { userId = identityUser.Id, token = emailConfirmationToken });
            //return RedirectToPage("/account/Login");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("RegistrationError_" + error.Code, error.Description);
        }

        return Page();
    }
}

public class RegisterViewModel
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    [Required]
    [DataType(dataType: DataType.Password)]
    public string Password { get; set; }
}