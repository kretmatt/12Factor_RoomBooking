using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RoomBooking.DataAccess;

public class RoomBookingContext:DbContext
{
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<Building> Buildings { get; set; }
    public DbSet<Room> Rooms { get; set; }

    public RoomBookingContext(DbContextOptions<RoomBookingContext> options):base(options)
    {
        //this.Database.EnsureDeleted();
        this.Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>().Navigation(x => x.Person).AutoInclude();
        modelBuilder.Entity<Booking>().Navigation(x => x.Room).AutoInclude();
        modelBuilder.Entity<Room>().Navigation(x => x.Building).AutoInclude();
        
        base.OnModelCreating(modelBuilder);
    }
}