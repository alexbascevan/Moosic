using Microsoft.EntityFrameworkCore;

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
* Purpose: Database context to handle database creation
* 
* API: https://reccobeats.com/docs/apis/get-recommendation
*/
    public class MoosicDbContext : DbContext
    {

        public MoosicDbContext(DbContextOptions<MoosicDbContext> options) : base(options) { }

        //Entity 
        public DbSet<User> Users { get; set; }

        //Entity 
        public DbSet<LibraryItem> LibraryItems { get; set; }

        //Entity
        public DbSet<Music> MusicItems { get; set; }

    }
}
