using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Authentics : ControllerBase
    {
        [HttpPost]
        public IActionResult Authenticate([FromBody] LoginCredentialsDTO credentialsDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Model state is not valid");
            if (credentialsDTO.Username != "admin" && credentialsDTO.Password != "Password")
                return Unauthorized("invalid username or password");

            //creating the security context 
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "admin@mywebsite.com"),
                new Claim(ClaimTypes.Name, "admin"),
                new Claim("Department", "HR"),
                new Claim("Admin", "true"),
                new Claim("Manager", "true"),
                new Claim("EmploymentDate", "2021-05-01"),
            };
            //create an identity from the claims
            //var identity = new ClaimsIdentity(claims);
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = credentialsDTO.RememberMe
            };

            var expiresAt = DateTime.Now.AddDays(2);
            return Ok(
                new
                {
                    access_token = CreateToken(claims: claims, expiresAt),
                    expires_at = expiresAt
                });
        }

        private string CreateToken(IEnumerable<Claim> claims, DateTime expires)
        {
            byte[] securityKeyBytes = Encoding.UTF8.GetBytes("123abcdefghijklmnopqrstuvwxyz12346512345678");
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKeyBytes);
            var hmacSignature = SecurityAlgorithms.HmacSha256Signature;

            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: new SigningCredentials(symmetricSecurityKey, hmacSignature));

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public class LoginCredentialsDTO
        {

            [Required]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember Me?")]
            public bool RememberMe { get; set; }
        }
    }
}