/*
* Group Name: Potatoes
* Project Name: Moosic
* 
* Created By: Mariah Falzon 
* 
* Created On: April 6, 2025
* Updated On: April 7 2025
* 
* Purpose: Home Controller to Display the API of Getting The New Albums
* Comments: REmoved the API for getting new releases wanted to include it on the Dashboard
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

 

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            
        }

      
        //Will Display New Releases On The Main Screen
       public async Task<IActionResult> Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
