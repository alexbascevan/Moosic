using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Moosic.Models
{
    /*
     * Group Name: Potatoes
     * Project Name: Moosic
     * 
     * Created By: Mariah Falzon 
     * 
     * Created On: April 6, 2025
     * Updated On: 
     * 
     * Purpose: Class to handle specific music information from the API
     * 
     * API: https://reccobeats.com/docs/apis/get-recommendation
     */
    public class Music
    {
        [Key]
        public int Id {  get; set; }    

        public string? Title { get; set; }

        public string? Artist { get; set; }

        public string? Album { get; set; }

        public string? Genre { get; set; }

        public string? ApiId { get; set; } //this will hold any of the IDs from the Recco Beats API
    }
}
