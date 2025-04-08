using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Moosic.Models
{
    /*
 * Group Name: Potatoes
 * Project Name: Moosic
 * 
 * Created By: Alexander Bascevan
 * 
 * Created On: April 6, 2025
 * Updated On: April 7 2025
 * 
 * Purpose: Class to handle specific user information 

 */
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string? userName { get; set; }

        [Required]
        [EmailAddress]
        public string? email { get; set; }

        [Required]
        public string? password { get; set; } 

        public List<LibraryItem>? LibraryItems { get; set; } //to get the infomation for the users library 
    }
}
