using Newtonsoft.Json;

namespace Learn_Asp_Net_Identity.DTO;

public class ApiLoginJsonResponseDTO
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    
    [JsonProperty("expires_at")]
    public DateTime ExpiresAt { get; set; }
}