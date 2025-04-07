using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Moosic.Models
{
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
