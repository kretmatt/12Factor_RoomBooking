using Common.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using RoomBooking.BusinessLogic.Interfaces;
using RoomBooking.DataAccess.Interfaces;

namespace RoomBooking.BusinessLogic;

public class PersonBusinessLogic:ISimpleCRUDLogic<Person>
{
    private IRoomBookingUnitOfWork _roomBookingUnitOfWork;
    private IValidator<Person> _personValidator;
    private ILogger<PersonBusinessLogic> _logger;
    
    public PersonBusinessLogic(IRoomBookingUnitOfWork roomBookingUnitOfWork, IValidator<Person> personValidator, ILogger<PersonBusinessLogic> logger)
    {
        _roomBookingUnitOfWork = roomBookingUnitOfWork;
        _personValidator = personValidator;
        _logger = logger;
    }
    
    public IEnumerable<Person> ReadAll()
    {
        IEnumerable<Person> people=null;

        try
        {
            _logger.LogInformation($"Attempting to retrieve people");
            people = _roomBookingUnitOfWork.PersonRepository.Get(null, null, "Bookings");
        }
        catch (Exception exception)
        {
            string errorMessage =
                "An error occurred in the Data Access Layer while retrieving all people. Can't continue in business logic layer";
            
            _logger.LogError(errorMessage);
            
            throw new Exception(
                errorMessage,
                exception);
        }

        return people;
    }

    public Person ReadById(int id)
    {
        Person person=null;

        try
        {
            _logger.LogInformation($"Attempting to retrieve person with id {id}");
            person = _roomBookingUnitOfWork.PersonRepository.GetById(id);
        }
        catch (Exception exception)
        {
            string errorMessage =
                $"An error occurred in the Data Access Layer while retrieving person with id {id}. Can't continue in business logic layer";
            
            _logger.LogError(errorMessage);
            
            throw new Exception(
                errorMessage,
                exception);
        }
        
        return person;
    }

    public void Create(Person entity)
    {
        string errorMessage;
        
        _logger.LogInformation("Validating the new person data");
        var validationResult = _personValidator.Validate(entity);
        
        if (!validationResult.IsValid)
        {
            errorMessage = "Validation of the person did not succeed!";
            _logger.LogError(errorMessage);
            throw new ValidationException(errorMessage, validationResult.Errors);
        }

        try
        {
            _logger.LogInformation("Attempting to save the new person");
            _roomBookingUnitOfWork.PersonRepository.Insert(entity);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to save the new person! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The person was saved");
    }

    public void Update(Person entity)
    {
        string errorMessage;
        
        _logger.LogInformation("Validating the updated person data");
        var validationResult = _personValidator.Validate(entity);
        
        if (!validationResult.IsValid)
        {
            errorMessage = "Validation of the person did not succeed!";
            _logger.LogError(errorMessage);
            throw new ValidationException(errorMessage, validationResult.Errors);
        }

        try
        {
            _logger.LogInformation("Attempting to update an existing person");
            _roomBookingUnitOfWork.PersonRepository.Update(entity);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to update the person! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The person was updated");
    }

    public void Delete(int id)
    {
        string errorMessage;
        
        try
        {
            _logger.LogInformation("Attempting to delete the person");
            var personToRemove = _roomBookingUnitOfWork.PersonRepository.GetById(id);
            _roomBookingUnitOfWork.PersonRepository.Delete(personToRemove);
            _roomBookingUnitOfWork.Save();
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to delete the person! Can't continue";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The person was deleted");
    }
}