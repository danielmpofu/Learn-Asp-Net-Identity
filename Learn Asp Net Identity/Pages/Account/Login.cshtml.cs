using System.Security.Claims;
using Learn_Asp_Net_Identity.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn_Asp_Net_Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty] public LoginCredentialsDTO credentialsDTO { get; set; }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            if (credentialsDTO.Username != "admin" && credentialsDTO.Password != "Password") return Page();
            
            //creating the security context 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                new Claim(ClaimTypes.Name, "admin")
            };
            //create an identity from the claims
            var identity = new ClaimsIdentity(claims);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookie", claimsPrincipal);
            return Redirect("/Index");
        }
    }
}