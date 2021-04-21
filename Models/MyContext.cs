using Microsoft.EntityFrameworkCore;
namespace WeddingPlanner.Models{

public class MyContext : DbContext{

    public MyContext(DbContextOptions Options) : base(Options){}

    public DbSet<User> Users { get; set; }
    public DbSet<Guest> Guests { get; set; }

    public DbSet<Wedding> Weddings { get; set; }

}
}