using Common.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RoomBooking.BusinessLogic.Interfaces;
using RoomBooking.DataAccess.Interfaces;

namespace RoomBooking.BusinessLogic;

public class BookingBusinessLogic:ISimpleCRUDLogic<Booking>
{
    private IRoomBookingUnitOfWork _roomBookingUnitOfWork;
    private IValidator<Booking> _bookingValidator;
    private ILogger<BookingBusinessLogic> _logger;

    public BookingBusinessLogic(IRoomBookingUnitOfWork roomBookingUnitOfWork, IValidator<Booking> bookingValidator, ILogger<BookingBusinessLogic> logger)
    {
        _roomBookingUnitOfWork = roomBookingUnitOfWork;
        _bookingValidator = bookingValidator;
        _logger = logger;
    }
    
    public IEnumerable<Booking> ReadAll()
    {
        IEnumerable<Booking> bookings;

        try
        {
            _logger.LogInformation($"Attempting to retrieve all bookings");
            bookings = _roomBookingUnitOfWork.BookingRepository.Get(null, null, "Room,Person");
        }
        catch (Exception exception)
        {
            string errorMessage =
                "An error occurred in the Data Access Layer while retrieving all bookings. Can't continue in business logic layer";
            
            _logger.LogError(errorMessage);
            
            throw new Exception(
                errorMessage,
                exception);
        }

        return bookings;
    }

    public Booking ReadById(int id)
    {
        Booking booking;

        try
        {
            _logger.LogInformation($"Attempting to retrieve booking with id {id}");
            booking = _roomBookingUnitOfWork.BookingRepository.GetById(id);
        }
        catch (Exception exception)
        {
            string errorMessage =
                $"An error occurred in the Data Access Layer while retrieving booking with id {id}. Can't continue in business logic layer";
            
            _logger.LogError(errorMessage);
            
            throw new Exception(
                errorMessage,
                exception);
        }
        
        return booking;
    }

    public void Create(Booking entity)
    {
        string errorMessage;
        
        _logger.LogInformation("Validating the new booking data");
        var validationResult = _bookingValidator.Validate(entity);
        
        if (!validationResult.IsValid)
        {
            errorMessage = "Validation of the booking did not succeed!";
            _logger.LogError(errorMessage);
            throw new ValidationException(errorMessage, validationResult.Errors);
        }

        try
        {
            _logger.LogInformation("Attempting to save the new booking");
            _roomBookingUnitOfWork.BookingRepository.Insert(entity);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to save the new booking! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The booking was saved");
    }

    public void Update(Booking entity)
    {
        string errorMessage;
        
        _logger.LogInformation("Validating the updated booking data");
        var validationResult = _bookingValidator.Validate(entity);
        
        if (!validationResult.IsValid)
        {
            errorMessage = "Validation of the booking did not succeed!";
            _logger.LogError(errorMessage);
            throw new ValidationException(errorMessage, validationResult.Errors);
        }

        try
        {
            _logger.LogInformation("Attempting to update an existing booking");
            _roomBookingUnitOfWork.BookingRepository.Update(entity);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to update the booking! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The booking was updated");
    }

    public void Delete(int id)
    {
        string errorMessage;
        
        try
        {
            _logger.LogInformation("Attempting to delete the booking");
            var bookingToRemove = _roomBookingUnitOfWork.BookingRepository.GetById(id);
            _roomBookingUnitOfWork.BookingRepository.Delete(bookingToRemove);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to delete the booking! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The booking was deleted");
    }

    public void Dispose()
    {
        _roomBookingUnitOfWork.Dispose();
        GC.SuppressFinalize(this);
    }
}