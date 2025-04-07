/*
* Group Name: Potatoes
* Project Name: Moosic
* 
* Created By: Mariah Falzon 
* 
* Created On: April 6, 2025
* Updated On: 
* 
* Purpose: Class to handle adding any music or rating to users library
* 
* API: https://developer.spotify.com/documentation/web-api/tutorials/getting-started
*/

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Moosic.Models
{
    public class LibraryItem
    {
        [Key]
        public int LibraryId { get; set; }

        public int UserId { get; set; }
        public User? User {get; set;}
        public int MusicId {  get; set; }
        public Music? Music { get; set; } //Foreign Key

        public int? Rating { get; set; } //rating of item on 5 star rating but can be optional

        public DateTime dateAdded { get; set; } //time for when the song gets added 

    }
}
