/*
* Group Name: Potatoes
* Project Name: Moosic
* 
* Created By: Alexander Bascevan
* 
* Created On: April 7, 2025
* Updated On: 
* 
* Purpose: Music Controller Class to handle searching for music, ratings and adding to library
* 
* Comments: Since I added more API information, I had to update the methods - mariah
* 
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moosic.Models;
using Moosic.Services;
using Microsoft.AspNetCore.Http;  // for Session

namespace Moosic.Controllers
{
    public class MusicController : Controller
    {
        private readonly ApiService _apiService;
        private readonly MoosicDbContext _context;

        public MusicController(ApiService apiService, MoosicDbContext context)
        {
            _apiService = apiService;
            _context = context;
        }

        // GET: /Music/Search
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        // GET: /Music/SearchResults?query=SearchTerm
        [HttpGet]
        public async Task<IActionResult> SearchResults(string query, string type = "track") //add a type 
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Music>());
            }

            var searchResults = await _apiService.SearchForMusic(query, type.ToLower()); // Searching for track, album or artist
            return View(searchResults);
        }

        // GET: /Music/AddToLibrary/{id}?title=...&artist=...&album=...&imageUrl=...
        [HttpGet]
        public async Task<IActionResult> AddToLibrary(string id, string title, string artist, string album, string imageUrl, string totalTrack, string releaseDate, string popularity)
        {
            // Retrieve logged-in user's id from session
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if the music record already exists in our local DB using the ApiId.
            var musicRecord = await _context.MusicItems.FirstOrDefaultAsync(m => m.ApiId == id);
            if (musicRecord == null)
            {
                // Create and save a new Music record using the provided details.
                musicRecord = new Music
                {
                    Title = title,
                    Artist = artist,
                    Album = album,
                    ImageUrl = imageUrl,
                    totalTracks = totalTrack,  
                    releaseDate = releaseDate,
                    popularity = popularity,
                    ApiId = id
                };
                _context.MusicItems.Add(musicRecord);
                await _context.SaveChangesAsync();
            }

            // Create a new LibraryItem record for the user.
            var libraryItem = new LibraryItem
            {
                UserId = userId.Value,
                MusicId = musicRecord.Id,
                dateAdded = DateTime.Now
            };

            _context.LibraryItems.Add(libraryItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Library", "Account");
        }


        // GET: /Music/Rating?libraryId=...
        [HttpGet]
        public async Task<IActionResult> Rating(int libraryId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the library item with its associated Music.
            var libraryItem = await _context.LibraryItems
                .Include(li => li.Music)
                .FirstOrDefaultAsync(li => li.LibraryId == libraryId && li.UserId == userId);
            if (libraryItem == null)
            {
                return NotFound();
            }
            return View(libraryItem);
        }

        // POST: /Music/Rating
        [HttpPost]
        public async Task<IActionResult> Rating(int libraryId, int rating)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var libraryItem = await _context.LibraryItems
                .FirstOrDefaultAsync(li => li.LibraryId == libraryId && li.UserId == userId);
            if (libraryItem == null)
            {
                return NotFound();
            }

            // Update the rating (value between 1 and 5)
            libraryItem.Rating = rating;
            _context.LibraryItems.Update(libraryItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Library", "Account");
        }
    }
}
