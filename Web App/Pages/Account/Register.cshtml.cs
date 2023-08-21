using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_App.Data;

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
        var identityUser = new AppUser
        {
            Email = RegisterViewModel.Email,
            UserName = SplitEmail(RegisterViewModel.Email),
            // Department = RegisterViewModel.Department,
            //Position = RegisterViewModel.Position
        };

        var deptClaim = new Claim("Department", RegisterViewModel.Department);
        var positionClaim = new Claim("Position", RegisterViewModel.Position);

        var result = await _userManager.CreateAsync(identityUser, RegisterViewModel.Password);
        if (result.Succeeded)
        {

            await _userManager.AddClaimAsync(user: identityUser, claim: deptClaim);
            await _userManager.AddClaimAsync(user: identityUser, claim: positionClaim);
            
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

            var link = Url.PageLink(pageName: "/Account/ConfirmEmail", values:
                new { userId = identityUser.Id, token = emailConfirmationToken });
            var message = new MailMessage(
                from: "turnaker@gmail.com",
                to: identityUser.Email,
                subject: "Please confirm your email address",
                body: $"Please click this link to confirm your email {link}");

            using (var emailClient = new SmtpClient(host: "smtp-relay.brevo.com", port: 587))
            {
                emailClient.Credentials = new NetworkCredential(userName: "turnaker@gmail.com",
                    password: "21sgJbQqMR65dI3a");
                await emailClient.SendMailAsync(message: message);
            }

            return RedirectToPage("/account/Login");
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

    [Required] public string Department { get; set; }

    [Required] public string Position { get; set; }
}