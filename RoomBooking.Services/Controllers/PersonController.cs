using System.Net;
using Common.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.BusinessLogic.Interfaces;

namespace RoomBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController:ControllerBase, IDisposable
{
    private ILogger<PersonController> _logger;
    private ISimpleCRUDLogic<Person> _personCRUDLogic;

    public PersonController(ISimpleCRUDLogic<Person> personCrudLogic, ILogger<PersonController> logger)
    {
        _logger = logger;
        _personCRUDLogic = personCrudLogic;
    }

    [HttpPost]
    public IActionResult Create(Person person)
    {
        try
        {
            _logger.LogInformation($"Received create request for a new person");
            _personCRUDLogic.Create(person);
        }
        catch (ValidationException ve)
        {
            _logger.LogError($"Person data could not be validated successfully. Errors:{ve}");
            return BadRequest("Bad person data");
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Data was validated, but led to other errors. Error: {e}");
            return BadRequest();
        }
        _logger.LogInformation("Person was created");
        return StatusCode(201,person);
    }

    [HttpGet("{id}")]
    public IActionResult Read(int id)
    {
        string errorMessage;
        try
        {
            _logger.LogInformation($"Received read request for person {id}");
            Person person = _personCRUDLogic.ReadById(id);
            
            if (person == null)
            {
                errorMessage = $"Person {id} not found";
                _logger.LogError(errorMessage);
                return NotFound();
            }
                
            _logger.LogInformation($"Person {id} was found");
            return Ok(person);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Retrieving person {id} led to an unexpected error. Error: {e}");
            return StatusCode(500);
        }
    }
    
    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _logger.LogInformation($"Received request to delete a person");
            _personCRUDLogic.Delete(id);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Error occurred while deleting person. Error: {e}");
            return StatusCode(500);
        }
        _logger.LogInformation("Person was deleted");

        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Person person)
    {
        try
        {
            _logger.LogInformation($"Received request to update a person");

            if (id != person.PersonId)
            {
                _logger.LogError("IDs are mismatching");
                return BadRequest();
            }
            
            _personCRUDLogic.Update(person);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Error occurred while updating person. Error: {e}");
            return StatusCode(500);
        }
        _logger.LogInformation("Person was updated");

        return Ok();
    }


    [HttpGet]
    public IActionResult ReadAll()
    {
        try
        {
            _logger.LogInformation($"Received read request for people");
            IEnumerable<Person> people = _personCRUDLogic.ReadAll();
            _logger.LogInformation($"People were retrieved");
            
            return Ok(people);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Retrieving people led to an unexpected error. Error: {e}");
            return StatusCode(500);
        }
    }
    
    public void Dispose()
    {
        _personCRUDLogic.Dispose();
        GC.SuppressFinalize(this);
    }
}