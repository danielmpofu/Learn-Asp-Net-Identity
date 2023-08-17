using Learn_Asp_Net_Identity.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn_Asp_Net_Identity.Pages
{
    [Authorize(policy:"HrManagerOnly")]
    public class HRManagerModel : PageModel
    {
        [BindProperty] 
        public List<WeatherForecastDTO> WeatherForecastDtos { get; set; }

        private readonly IHttpClientFactory _httpClientFactory;

        public HRManagerModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> OnGetAsync()
        {
          var httpClient =  _httpClientFactory.CreateClient("OurWebApiClient");
          WeatherForecastDtos =  await httpClient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast");
          return Page();
        }
    }
}
