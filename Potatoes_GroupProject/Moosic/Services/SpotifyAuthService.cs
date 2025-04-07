using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Moosic.Services
{
    /*
 * Group Name: Potatoes
 * Project Name: Moosic
 * 
 * Created By: Mariah Falzon 
 * 
 * Created On: April 6, 2025
 * Updated On: s
 * 
 * Purpose: Service Class to call the Spotify Authorization
 * 
 * API: https://developer.spotify.com/documentation/web-api/tutorials/getting-started
 */
    public class SpotifyAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public SpotifyAuthService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient; //send HTTP requests
            _config = config; //read Client ID and Client Secret from appsettings.json
        }

        public async Task<string> GetAccessTokenAsync()
        {
            //Method receives access tokens

            //Using Documentation from Spotify and https://curlconverter.com/csharp/ 

            /*
             * curl request is
             * curl -X POST "https://accounts.spotify.com/api/token" \
                     -H "Content-Type: application/x-www-form-urlencoded" \
                     -d "grant_type=client_credentials&client_id=your-client-id&client_secret=your-client-secret"

             * 
             */

            //authenticate app with Spotify and get token
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _config["Spotify:ClientId"] },
            { "client_secret", _config["Spotify:ClientSecret"] }
        });

            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            return json.GetProperty("access_token").GetString();
        }
    }
}
