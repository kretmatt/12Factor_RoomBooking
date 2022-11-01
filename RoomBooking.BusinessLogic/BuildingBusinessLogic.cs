using Common.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RoomBooking.BusinessLogic.Interfaces;
using RoomBooking.DataAccess.Interfaces;

namespace RoomBooking.BusinessLogic;

public class BuildingBusinessLogic:ISimpleCRUDLogic<Building>
{
    private IRoomBookingUnitOfWork _roomBookingUnitOfWork;
    private IValidator<Building> _buildingValidator;
    private ILogger<BuildingBusinessLogic> _logger;
    
    public BuildingBusinessLogic(IRoomBookingUnitOfWork roomBookingUnitOfWork, IValidator<Building> buildingValidator, ILogger<BuildingBusinessLogic> logger)
    {
        _roomBookingUnitOfWork = roomBookingUnitOfWork;
        _buildingValidator = buildingValidator;
        _logger = logger;
    }
    
    public IEnumerable<Building> ReadAll()
    {
        IEnumerable<Building> buildings;

        try
        {
            _logger.LogInformation($"Attempting to retrieve buildings");
            buildings = _roomBookingUnitOfWork.BuildingRepository.Get(null, null, "Rooms");
        }
        catch (Exception exception)
        {
            string errorMessage =
                "An error occurred in the Data Access Layer while retrieving all buildings. Can't continue in business logic layer";
            
            _logger.LogError(errorMessage);
            
            throw new Exception(
                errorMessage,
                exception);
        }

        return buildings;
    }

    public Building ReadById(int id)
    {
        Building building;

        try
        {
            _logger.LogInformation($"Attempting to retrieve building with id {id}");
            building = _roomBookingUnitOfWork.BuildingRepository.GetById(id);
        }
        catch (Exception exception)
        {
            string errorMessage =
                $"An error occurred in the Data Access Layer while retrieving building with id {id}. Can't continue in business logic layer";
            
            _logger.LogError(errorMessage);
            
            throw new Exception(
                errorMessage,
                exception);
        }
        
        return building;
    }

    public void Create(Building entity)
    {
        string errorMessage;
        
        _logger.LogInformation("Validating the new building data");
        var validationResult = _buildingValidator.Validate(entity);
        
        if (!validationResult.IsValid)
        {
            errorMessage = "Validation of the building did not succeed!";
            _logger.LogError(errorMessage);
            throw new ValidationException(errorMessage, validationResult.Errors);
        }

        try
        {
            _logger.LogInformation("Attempting to save the new building");
            _roomBookingUnitOfWork.BuildingRepository.Insert(entity);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to save the new building! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The building was saved");
    }

    public void Update(Building entity)
    {
        string errorMessage;
        
        _logger.LogInformation("Validating the updated building data");
        var validationResult = _buildingValidator.Validate(entity);
        
        if (!validationResult.IsValid)
        {
            errorMessage = "Validation of the building did not succeed!";
            _logger.LogError(errorMessage);
            throw new ValidationException(errorMessage, validationResult.Errors);
        }

        try
        {
            _logger.LogInformation("Attempting to update an existing building");
            _roomBookingUnitOfWork.BuildingRepository.Update(entity);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to update the building! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The building was updated");
    }

    public void Delete(int id)
    {
        string errorMessage;
        
        try
        {
            _logger.LogInformation("Attempting to delete the building");
            var buildingToRemove = _roomBookingUnitOfWork.BuildingRepository.GetById(id);
            _roomBookingUnitOfWork.BuildingRepository.Delete(buildingToRemove);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to delete the building! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The building was deleted");
    }

    public void Dispose()
    {
        _roomBookingUnitOfWork.Dispose();
        GC.SuppressFinalize(this);
    }
}