using Moosic.Models;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Moosic.Services
{

    /*
 * Group Name: Potatoes
 * Project Name: Moosic
 * 
 * Created By: Mariah Falzon 
 * 
 * Created On: April 6, 2025
 * Updated On: April 7 2025
 * 
 * Purpose: Service Class to call the API information
 * 
 * API: https://developer.spotify.com/documentation/web-api/tutorials/getting-started
 * 
 * 
 * https://developer.spotify.com/documentation/web-api/reference/search
 * 
 * Comments: Updating the API Service with extra API Details
 */
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly SpotifyAuthService _authService;

        public ApiService(HttpClient httpClient, SpotifyAuthService authService)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.spotify.com/v1/");
            _authService = authService;
        }

        // Helper method to add auth token to requests
        private async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            var token = await _authService.GetAccessTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            return _httpClient;
        }

        //Search Spotify For Items 
        //query for the specific search result 
        //type for the specific type parameter 
        public async Task<List<Music>> SearchForMusic(string query, string type)
        {
            var client = await GetAuthenticatedClientAsync(); //await the token request

            //search is being added to the base URI, with the query in the EscapeDataString, and the type will be added 
            var url = $"search?q={Uri.EscapeDataString(query)}&type={type.ToLower()}&limit=10";
            var response = await client.GetAsync(url);

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();


            var results = new List<Music>(); // create a list for the results of the searched music


            //track Search 
            if(type.Equals("track", StringComparison.OrdinalIgnoreCase))
            {
                var tracks = json.GetProperty("tracks").GetProperty("items");

                foreach( var track in tracks.EnumerateArray())
                {
                    results.Add(new Music
                    {
                        ApiId = track.GetProperty("id").GetString(), //id property to the APIid
                        Title = track.GetProperty("name").GetString(), //name of track to the title
                        Artist = string.Join(", ", track.GetProperty("artists") //name of artist to artist in music
                        .EnumerateArray()
                        .Select(artist => artist.GetProperty("name").GetString())),
                        Album = track.GetProperty("album").GetProperty("name").GetString(),
                        releaseDate = track.GetProperty("album").GetProperty("release_date").GetString(),
                        totalTracks = track.GetProperty("album").GetProperty("total_tracks").GetInt32().ToString(),
                        popularity = track.GetProperty("popularity").GetInt32().ToString(),
                        ImageUrl = track.GetProperty("album").GetProperty("images")
                            .EnumerateArray().FirstOrDefault().GetProperty("url").GetString() //getting the URL from the images JSON
                   
                    });
                }
            }

            //Album Search

            if (type.Equals("album", StringComparison.OrdinalIgnoreCase))
            {
                var albums = json.GetProperty("albums").GetProperty("items");

                foreach (var a in albums.EnumerateArray())
                {
                    results.Add(new Music
                    {
                        ApiId = a.GetProperty("id").GetString(), //id property to the APIid
                        Title = null, //no track title
                        Artist = string.Join(", ", a.GetProperty("artists") //name of artist to artist in music
                        .EnumerateArray()
                        .Select(artist => artist.GetProperty("name").GetString())),
                        Album = a.GetProperty("name").GetString(), //name of album
                        releaseDate = a.GetProperty("release_date").GetString(),
                        totalTracks = a.GetProperty("total_tracks").GetInt32().ToString(),
                        popularity = null, //no popularity for the album in API
                        ImageUrl = a.GetProperty("images")
                            .EnumerateArray().FirstOrDefault().GetProperty("url").GetString() //getting the URL from the images JSON

                    });
                }
            }
          
            return results;
        }


        //Get New Releases 
        //https://developer.spotify.com/documentation/web-api/reference/get-new-releases

        public async Task<List<Music>> GetNewReleases()
        {
            var client = await GetAuthenticatedClientAsync();

            var response = await client.GetFromJsonAsync<JsonElement>("browse/new-releases");

            var musicList = new List<Music>();


            foreach (var a in response.GetProperty("albums").GetProperty("items").EnumerateArray())
            {
                musicList.Add(new Music
                {
                    ApiId = a.GetProperty("id").GetString(), //id property to the APIid
                    Title = null, //no track title
                    Artist = string.Join(", ", a.GetProperty("artists") //name of artist to artist in music
                        .EnumerateArray()
                        .Select(artist => artist.GetProperty("name").GetString())),
                    Album = a.GetProperty("name").GetString(), //name of album
                    releaseDate = a.GetProperty("release_date").GetString(),
                    totalTracks = a.GetProperty("total_tracks").GetInt32().ToString(),
                    popularity = null, //no popularity for the album in API
                    ImageUrl = a.GetProperty("images")
                            .EnumerateArray().FirstOrDefault().GetProperty("url").GetString() //getting the URL from the images JSON
                });
            }

            return musicList;
        }
    }

}
