using Microsoft.Extensions.Configuration;
using PlacesApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlacesApp.Services.Data
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private string _accessToken;
        private DateTime _tokenExpiryTime;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _clientId = configuration["Auth:ClientId"];
            _clientSecret = configuration["Auth:ClientSecret"];
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _tokenExpiryTime)
            {
                await RefreshAccessTokenAsync();
            }
            return _accessToken;
        }

        private async Task RefreshAccessTokenAsync()
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://staging.identity.eos.kerridgecs.online/connect/token");
            tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _clientId,
                ["client_secret"] = _clientSecret,
                ["grant_type"] = "client_credentials",
                ["scope"] = "eos.common.fullaccess"
            });

            var response = await _httpClient.SendAsync(tokenRequest);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);

            _accessToken = tokenResponse.AccessToken;
            _tokenExpiryTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
        }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
