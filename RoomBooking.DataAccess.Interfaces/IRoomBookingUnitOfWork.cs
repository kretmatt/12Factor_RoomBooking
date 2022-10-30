using Common.Entities;

namespace RoomBooking.DataAccess.Interfaces;

public interface IRoomBookingUnitOfWork : IDisposable
{
    IDALRepository<Room> RoomRepository
    {
        get;
    }

    IDALRepository<Booking> BookingRepository
    {
        get;
    }

    IDALRepository<Building> BuildingRepository
    {
        get;
    }

    IDALRepository<Person> PersonRepository
    {
        get;
    }

    void Save();
}