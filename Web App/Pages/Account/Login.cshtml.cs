using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_App.Pages.Account;

public class Login : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    [BindProperty] 
    public LoginCredentialsDTO credentialsDTO { get; set; }

    public void OnGet()
    {
    }

//rosaria.muteketo@sc.com
    public Login(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var signInResult = await _signInManager.PasswordSignInAsync(
            userName: credentialsDTO.Email,
            
            password: credentialsDTO.Password,
            isPersistent: credentialsDTO.RememberMe,
            lockoutOnFailure: false
        );
        if(signInResult.IsLockedOut) ModelState.AddModelError("Locked Account", "You are locked out");
        if (signInResult.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        else
        {
            ModelState.AddModelError("Error","Invalid password and username combination");
            return Page();
        }
    }
    
}

public class LoginCredentialsDTO
{
    [Required] public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Remember Me?")] public bool RememberMe { get; set; }
}

