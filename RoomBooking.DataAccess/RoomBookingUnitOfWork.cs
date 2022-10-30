using Common.Entities;
using Microsoft.Extensions.Logging;
using RoomBooking.DataAccess.Interfaces;

namespace RoomBooking.DataAccess;

public class RoomBookingUnitOfWork:IRoomBookingUnitOfWork
{
    private RoomBookingContext _context;
    private bool _disposed;
    private ILogger<RoomBookingUnitOfWork> _logger;

    public RoomBookingUnitOfWork(RoomBookingContext context, 
        IDALRepository<Room> roomRepository,
        IDALRepository<Booking> bookingRepository,
        IDALRepository<Person> personRepository,
        IDALRepository<Building> buildingRepository,
        ILogger<RoomBookingUnitOfWork> logger)
    {
        _context = context;
        BookingRepository = bookingRepository;
        RoomRepository = roomRepository;
        BuildingRepository = buildingRepository;
        PersonRepository = personRepository;
        _disposed = false;
        _logger = logger;
    }
    
    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Dispose();
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public IDALRepository<Room> RoomRepository { get; }

    public IDALRepository<Booking> BookingRepository { get; }

    public IDALRepository<Building> BuildingRepository { get; }

    public IDALRepository<Person> PersonRepository { get; }

    public void Save()
    {
        _logger.LogInformation("Saving changes to the database");
        _context.SaveChanges();
    }
}