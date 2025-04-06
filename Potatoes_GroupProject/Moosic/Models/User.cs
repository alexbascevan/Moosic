using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Moosic.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string? userName { get; set; }

        public string? email { get; set; }

        public string? password { get; set; } 

        public List<LibraryItem>? LibraryItems { get; set; } //to get the infomation for the users library 
    }
}
