/*
* Group Name: Potatoes
* Project Name: Moosic
* 
* Created By: Mariah Falzon 
* 
* Created On: April 6, 2025
* Updated On: s
* 
* Purpose: Home Controller to Display the API of Getting The New Albums
* 
* 
*/

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Moosic.Models;
using Moosic.Services;

namespace Moosic.Controllers
{   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //Injecting the APIService I created
        private readonly ApiService _apiService;

        public HomeController(ILogger<HomeController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

      
        //Will Display New Releases On The Main Screen
       public async Task<IActionResult> Index()
        {
            var releases = await _apiService.GetNewReleases();
            return View(releases);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
