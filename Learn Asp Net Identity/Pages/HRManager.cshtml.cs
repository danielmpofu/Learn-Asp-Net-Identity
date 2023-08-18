using System.Net.Http.Headers;
using Learn_Asp_Net_Identity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Learn_Asp_Net_Identity.Pages
{
    [Authorize(policy: "HrManagerOnly")]
    public class HRManagerModel : PageModel
    {
        [BindProperty] public List<WeatherForecastDTO> WeatherForecastDtos { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public HRManagerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task<ApiLoginJsonResponseDTO> GetToken()
        {
            var httpClient = _httpClientFactory.CreateClient("OurWebApiClient");
            var loginResponse = await httpClient.PostAsJsonAsync("auth",
                new LoginCredentialsDTO { Username = "admin", Password = "Password", RememberMe = true });
            loginResponse.EnsureSuccessStatusCode();
            var jwtTokenResponse = await loginResponse.Content.ReadAsStringAsync();
            //set the token string to sessions
            HttpContext.Session.SetString("access_token", jwtTokenResponse);
            return JsonConvert.DeserializeObject<ApiLoginJsonResponseDTO>(jwtTokenResponse);
        }

        private async Task<T> InvokeApiEndPoint<T>(string clientName, string url)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var tokenObjectFromSession = HttpContext.Session.GetString("access_token");
            var tokenObj = new ApiLoginJsonResponseDTO();
            //get token from session

            if (string.IsNullOrWhiteSpace(tokenObjectFromSession))
            {
                tokenObj = await GetToken();
            }

            if (string.IsNullOrWhiteSpace(tokenObj.AccessToken) || tokenObj.ExpiresAt <= DateTime.Now)
            {
                //that token is failing go and get a new one
                tokenObj = await GetToken();
            }
            //authentication and getting the token

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenObj.AccessToken);
            return await httpClient.GetFromJsonAsync<T>(url);  
        }

        public async Task<IActionResult> OnGetAsync()
        {
            WeatherForecastDtos = await InvokeApiEndPoint<List<WeatherForecastDTO>>("OurWebApiClient","WeatherForecast");
            return Page();
        }
    }
}