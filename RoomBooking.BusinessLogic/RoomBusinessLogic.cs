using Common.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RoomBooking.BusinessLogic.Interfaces;
using RoomBooking.DataAccess.Interfaces;

namespace RoomBooking.BusinessLogic;

public class RoomBusinessLogic:ISimpleCRUDLogic<Room>
{
    private IRoomBookingUnitOfWork _roomBookingUnitOfWork;
    private IValidator<Room> _roomValidator;
    private ILogger<RoomBusinessLogic> _logger;
    
    public RoomBusinessLogic(IRoomBookingUnitOfWork roomBookingUnitOfWork, IValidator<Room> roomValidator, ILogger<RoomBusinessLogic> logger)
    {
        _roomBookingUnitOfWork = roomBookingUnitOfWork;
        _roomValidator = roomValidator;
        _logger = logger;
    }
    
    public IEnumerable<Room> ReadAll()
    {
        IEnumerable<Room> rooms=null;

        try
        {
            _logger.LogInformation($"Attempting to retrieve rooms");
            rooms = _roomBookingUnitOfWork.RoomRepository.Get(null, null, "Bookings");
        }
        catch (Exception exception)
        {
            string errorMessage =
                "An error occurred in the Data Access Layer while retrieving all rooms. Can't continue in business logic layer";
            
            _logger.LogError(errorMessage);
            
            throw new Exception(
                errorMessage,
                exception);
        }

        return rooms;
    }

    public Room ReadById(int id)
    {
        Room room=null;

        try
        {
            _logger.LogInformation($"Attempting to retrieve room with id {id}");
            room = _roomBookingUnitOfWork.RoomRepository.GetById(id);
        }
        catch (Exception exception)
        {
            string errorMessage =
                $"An error occurred in the Data Access Layer while retrieving room with id {id}. Can't continue in business logic layer";
            
            _logger.LogError(errorMessage);
            
            throw new Exception(
                errorMessage,
                exception);
        }
        
        return room;
    }

    public void Create(Room entity)
    {
        string errorMessage;
        
        _logger.LogInformation("Validating the new room data");
        var validationResult = _roomValidator.Validate(entity);
        
        if (!validationResult.IsValid)
        {
            errorMessage = "Validation of the room did not succeed!";
            _logger.LogError(errorMessage);
            throw new ValidationException(errorMessage, validationResult.Errors);
        }

        try
        {
            _logger.LogInformation("Attempting to save the new room");
            _roomBookingUnitOfWork.RoomRepository.Insert(entity);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to save the new room! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The room was saved");
    }

    public void Update(Room entity)
    {
        string errorMessage;
        
        _logger.LogInformation("Validating the updated room data");
        var validationResult = _roomValidator.Validate(entity);
        
        if (!validationResult.IsValid)
        {
            errorMessage = "Validation of the room did not succeed!";
            _logger.LogError(errorMessage);
            throw new ValidationException(errorMessage, validationResult.Errors);
        }

        try
        {
            _logger.LogInformation("Attempting to update an room");
            _roomBookingUnitOfWork.RoomRepository.Update(entity);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to update the room! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The room was updated");
    }

    public void Delete(int id)
    {
        string errorMessage;
        
        try
        {
            _logger.LogInformation("Attempting to delete the room");
            var roomToRemove = _roomBookingUnitOfWork.RoomRepository.GetById(id);
            _roomBookingUnitOfWork.RoomRepository.Delete(roomToRemove);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to delete the room! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The room was deleted");
    }

    public void Dispose()
    {
        _roomBookingUnitOfWork.Dispose();
        GC.SuppressFinalize(this);
    }
}