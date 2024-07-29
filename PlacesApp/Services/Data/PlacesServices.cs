using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using PlacesApp.Services.Interfaces;
using PlacesApp.Model;
using System.Globalization;

namespace PlacesApp.Services.Data
{
    public class PlacesService : IPlacesService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly string _baseUrl = "https://staging.api.eos.kerridgecs.online/location";

        public PlacesService(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private async Task EnsureAuthorizationHeader()
        {
            var token = await _authService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<Place>> SearchPlaces(string query)
        {
            await EnsureAuthorizationHeader();
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/locations/places?criteria={Uri.EscapeDataString(query)}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var placesResponse = JsonConvert.DeserializeObject<PlacesResponse>(content);
            return placesResponse?.Data ?? new List<Place>();
        }

        public async Task<PlaceDetails> GetPlaceDetails(string placeId)
        {
            await EnsureAuthorizationHeader();
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/locations/places/{placeId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var placeDataResponse = JsonConvert.DeserializeObject<PlaceDetailsResponse>(content);
            return placeDataResponse?.Data ?? new PlaceDetails();
        }

        public async Task<string> GetPhoto(string photoId)
        {
            await EnsureAuthorizationHeader();
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/locations/photos/{photoId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var photoResponse = JsonConvert.DeserializeObject<PhotoResponse>(content);
            return photoResponse?.Data.Photo;
        }

        public async Task<List<PlaceDetails>> FindNearbyPlaces(double latitude, double longitude, int radius = 2000)
        {
            await EnsureAuthorizationHeader();
            var url = $"{_baseUrl}/api/v1/locations/places/nearby?latitude={latitude.ToString(CultureInfo.InvariantCulture)}&longitude={longitude.ToString(CultureInfo.InvariantCulture)}&radius={radius}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var placesResponse = JsonConvert.DeserializeObject<NearbyPlacesResponse>(content);
            return placesResponse?.Data ?? new List<PlaceDetails>();
        }
    }
}